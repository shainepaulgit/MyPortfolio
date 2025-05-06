const viewersPerMonth = document.getElementById('viewers-per-month').getContext('2d');
const requestGiHubInvitationStatus = document.getElementById("request-github-invitation-status").getContext('2d');
const skillDataTable = $("#skill-table-data");
const serviceCategoryDataTable = $("#service-category-table-data");
const projectDataTable = $("#project-table-data");
const projectCategoryDataTable = $("#project-category-table-data");
const submitViewComponentPlaceholder = $("#submit-view-component-placeholder");
const deleteRowModal = $("#delete-row-modal");
let submitFormObj = {
    viewComponentController: '',
    modalId: ''
};

menuItem5.addClass("active");

$.ajax({
    url: "/AdminPage?handler=GitHubRequestsStatus",
    type: "GET",
    datatype: "json",
    success: function (result) {
        console.log(result);
        const pieGraphData = {
            labels: [
                'Pending',
                'Accepted',
                'Declined',
              
            ],
            datasets: [{
                label: 'Total',
                data: [result.pendingRequests, result.acceptedRequests,result.declinedRequests],
                backgroundColor: [
                    '#8EAC50',
                    '#3A59D1',
                    '#D84040'
                ],
                hoverOffset: 4
            }]
        };
        new Chart(requestGiHubInvitationStatus, {
            type: 'doughnut',
            data: pieGraphData,
            options: {}
        })
    }
})



$.ajax({
    url: "/AdminPage?handler=ViewersPerMonth",
    type: "GET",
    datatype: "json",
    success: function (result) {
        const labels = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
        const lineGraphData = {
            labels: labels,
            datasets: [{
                label: 'Viewers Per Month',
                data: [
                    result.january,
                    result.february,
                    result.march,
                    result.april,
                    result.may,
                    result.june,
                    result.july,
                    result.august,
                    result.september,
                    result.october,
                    result.november,
                    result.december
                ],
                fill: false,
                borderColor: 'rgba(80, 0, 115, 0.7)',
                tension: 0.1
            }]
        };
        new Chart(viewersPerMonth, {
            type: 'line',
            data: lineGraphData,
            options: {}
        });

    }
})






function deleteRecordModal(id, type) {
    $("#delete-row-modal").modal("show");
    $("#delete-row-modal").find("input[name='recordId']").val(String(id));
    $("#delete-row-modal").find("input[name='type']").val(type);
}
function submitForm(submitFormObj) {
    $.ajax({
        url: submitFormObj.viewComponentController, // Ensure ?handler= syntax for Razor Pages
        type: 'GET',
        success: function (response) {
            submitViewComponentPlaceholder.html(response);
            $(submitFormObj.modalId).modal('show');
            $.validator.unobtrusive.parse($(submitFormObj.modalId).find('form'));
        }
    });
}









skillDataTable.DataTable({
    "processing": true,
    "ajax": {
        "url": "/AdminPage?handler=DataTable",
        "type": "GET",
        "dataSrc": "skills"
    },
    "columns": [
        {
            "data": "logoFileName",
            "render": function (data) {
                return `<div class="d-flex justify-content-center align-items-center">
                    <img src="/LogoPictureFiles/${data}" class="rounded-circle" width="60" height="60"/>
                </div>`;
            }
        },

        { "data": "title" },
        {
            "data": "skillCategory",
            "render": function (data) {
                let skillCategory = data == 0 ? "Native" : "Framework";
                return skillCategory;
            }
        },
        {
            "data": "skillPercentage",
            "render": function (data) {
                let color = data < 40 ? "text-danger" : "text-dark";
                return `<span class="${color} fw-semibold">${data}%</span>`;
            }
        },

        {
            "data": "id",
            "render": function (data) {
                return `
                    <div class="text-center">
                        <button onclick="addUpdateSkill(${data})" class="btn btn-success btn-sm rounded-3">
                            <i class="fa fa-pen-to-square"></i>
                        </button>
                        <button onclick="deleteRecordModal(${data},'skill')" class="btn btn-sm rounded-3 btn-danger">
                            <i class="fa fa-trash-can"></i>
                        </button>
                    </div>
        `;
            }
        }
    ],
    "order": [[3, "desc"]],
    "destroy": true
});
function addUpdateSkill(id) {
    submitFormObj.viewComponentController = `/AdminPage?handler=LoadViewComponentSubmit&view=SkillForm&id=${id}`;
    submitFormObj.modalId = `#add-update-skill`;
    submitForm(submitFormObj);
}


serviceCategoryDataTable.DataTable({
    "processing": true,
    "ajax": {
        "url": "/AdminPage?handler=DataTable",
        "type": "GET",
        "dataSrc": "serviceCategories"
    },
    "columns": [
        { "data": "id" },
        { "data": "title" },
        { "data": "description" },
        { "data": "iconName" },
        {
            "data": "id",
            "render": function (data) {
                return `
                    <div class="text-center">
                        <button onclick="addUpdateServiceCategory(${data})" class="btn btn-success btn-sm rounded-3">
                            <i class="fa fa-pen-to-square"></i>
                        </button>
                        <button onclick="deleteRecordModal(${data},'serviceCategory')" class="btn btn-sm rounded-3 btn-danger">
                            <i class="fa fa-trash-can"></i>
                        </button>
                    </div>
        `;
            }
        }
    ],
    "order": [[0, "desc"]],
    "destroy": true
});
function addUpdateServiceCategory(id) {
    submitFormObj.viewComponentController = `/AdminPage?handler=LoadViewComponentSubmit&view=ServiceCategoryForm&id=${id}`;
    submitFormObj.modalId = `#add-update-service-category`;
    submitForm(submitFormObj);
}


projectDataTable.DataTable({
    "processing": true,
    "ajax": {
        "url": "/AdminPage?handler=DataTable",
        "type": "GET",
        "dataSrc": "projects"
    },
    "columns": [
        { "data": "id" },
        { "data": "projectTitle" },
        {
            "data": "projectDescription"
        },
        {
            "data": "projectPictureFileName",
            "render": function (data) {
                return `<div class="dropdown">
                          <i class="fa fa-eye text-primary d-block text-center" fs-5 dropdown-toggle" type="button" data-bs-toggle="dropdown"></i>
                         
                          <div class="dropdown-menu p-0" style="width: 100px; height: 70px;">
                            <img src="/ProjectPictureFiles/${data}" class="w-100 h-100"/>
                          </div>
                        </div>
                        `;
            }
        },
        { "data": "redirectUrl" },
        { "data": "isGitHubRepository" },
        {
            "data": "id",
            "render": function (data) {
                return `
                    <div class="text-center">
                        <button onclick="addUpdateProject(${data})" class="btn btn-success btn-sm rounded-3">
                            <i class="fa fa-pen-to-square"></i>
                        </button>
                        <button onclick="deleteRecordModal(${data},'project')" class="btn btn-sm rounded-3 btn-danger">
                            <i class="fa fa-trash-can"></i>
                        </button>
                    </div>
        `;
            }
        }
    ],
    "order": [[0, "desc"]],
    "destroy": true
});
function addUpdateProject(id) {
    submitFormObj.viewComponentController = `/AdminPage?handler=LoadViewComponentSubmit&view=ProjectForm&id=${id}`;
    submitFormObj.modalId = `#add-update-project`;
    submitForm(submitFormObj);
}


projectCategoryDataTable.DataTable({
    "processing": true,
    "ajax": {
        "url": "/AdminPage?handler=DataTable",
        "type": "GET",
        "dataSrc": "projectCategories"
    },
    "columns": [
        { "data": "id" },
        { "data": "title" },
        { "data": "description" },
        { "data": "iconName" },
        //{
        //    "data": "createdAt",
        //    "render": function (data) {
        //        let date = new Date(data); 
        //        return date.toLocaleString('en-US', {
        //            month: 'long', 
        //            day: '2-digit',
        //            year: 'numeric',
        //            hour: '2-digit',
        //            minute: '2-digit',
        //            hour12: true
        //        }).replace('AM', 'am').replace('PM', 'pm'); 
        //    }
        //},
        //{
        //    "data": "updatedAt",
        //    "render": function (data) {
        //        let date = new Date(data); 
        //        return date.toLocaleString('en-US', {
        //            month: 'long',  
        //            day: '2-digit',
        //            year: 'numeric',
        //            hour: '2-digit',
        //            minute: '2-digit',
        //            hour12: true
        //        }).replace('AM', 'am').replace('PM', 'pm'); 
        //    }
        //},
        {
            "data": "id",
            "render": function (data) {
                return `
                    <div class="text-center">
                        <button onclick="addUpdateProjectCategory(${data})" class="btn btn-success btn-sm rounded-3">
                            <i class="fa fa-pen-to-square"></i>
                        </button>
                        <button onclick="deleteRecordModal(${data},'projectCategory')" class="btn btn-sm rounded-3 btn-danger">
                            <i class="fa fa-trash-can"></i>
                        </button>
                    </div>
        `;
            }
        }
    ],
    "order": [[0, "desc"]],
    "destroy": true
});
function addUpdateProjectCategory(id) {
    submitFormObj.viewComponentController = `/AdminPage?handler=LoadViewComponentSubmit&view=ProjectCategoryForm&id=${id}`;
    submitFormObj.modalId = `#add-update-project-category`;
    submitForm(submitFormObj);
}





