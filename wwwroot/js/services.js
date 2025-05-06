menuItem3.addClass("active");
let gitHubRequestModal = $("#github-request");
function modalShow(repoName) {
    gitHubRequestModal.modal("show");
    gitHubRequestModal.find("input:nth-child(1)").val(repoName);
}