let dataTable = $("#data-table"),
requestActionModal = $("#request-action");

menuItem6.addClass("active");
$.ajax({
    url: "/GitHubRequestPage?handler=GitHubRequests",
    type: "GET",
    dataType: "json",
    success: function (e) {
        console.log(e);
    }
})

dataTable.DataTable({
    "processing": true,
    "ajax": {
        "url": "/GitHubRequestPage?handler=GitHubRequests",
        "type": "GET",
        "dataSrc": ""
    },
    "columns": [
        { "data": "id" },
        { "data": "repositoryName" },
        { "data": "gitHubUserName" },
        {
            "data": "status",
            "render": function (e) {
                return e == 0 ? "Pending" : e == 1 ? "Approved" : "Declined";
            }
        },
        {
            "data": "createdAt",
            "render": function (data) {
                let date = new Date(data);
                return date.toLocaleString('en-US', {
                    month: 'long',
                    day: '2-digit',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit',
                    hour12: true
                }).replace('AM', 'am').replace('PM', 'pm');
            }
        },
        {
            "id": "id",
            "status": "status",
            "render": function (data, status, row) {
                return `
            <div class="text-center">
                ${row.status == 0 ? `
                <button onclick="requestAction(${row.id})" class="btn btn-info text-white btn-sm rounded-3">
                    <i class="fa fa-user-gear"></i>
                </button>` : ''}
                ${row.status > 0 ? ` <button onclick="deleteRecordModal(${row.id})" class="btn btn-sm rounded-3 btn-danger">
                    <i class="fa fa-trash-can"></i>
                </button>`:''}
               
            </div>
        `;
            }
        }

      
    ],
    "order": [[0, "desc"]],
    "destroy": true
});
function deleteRecordModal(id) {
    $("#delete-row-modal").modal("show");
    $("#delete-row-modal").find("input[name='recordId']").val(String(id));
}
function requestAction(id) {
    requestActionModal.modal('show');
    requestActionModal.find("input[name='requestId']").val(String(id));
}