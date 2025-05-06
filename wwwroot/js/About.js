let frameworkSkillsRow = $(".row-framework-skills");
let nativeSkillsRow = $(".row-native-skills");
menuItem2.addClass("active");
$.ajax({
    url: "/AboutPage?handler=Skills",
    dataType: "json",
    type: "GET",
    success: function (response) {
        response.nativeSkills.forEach(function (e) {
            let skill = `
            <div class="col-lg-4 col-md-6 col-12 mb-4 fade-in-hidden">
                <div class="bg-white shadow rounded-3 p-4 d-flex flex-column">
                    <img src="/LogoPictureFiles/${e.logoFileName}" class="rounded-4 mb-3" width="50" height="50" />
                    <h6 class="text-body-secondary mb-1 fw-bolder">${e.title}</h6>
                    <p class="text-body-tertiary mb-4">Below is the percentage of my skill in ${e.title}</p>
                    <span class="text-end d-block color-theme percentage-text">0%</span>
                    <div class="progress" role="progressbar" aria-label="Example 1px high" aria-valuenow="${e.skillPercentage}" aria-valuemin="0" aria-valuemax="100" style="height: 3px">
                        <div class="progress-bar bg-color-theme" style="width: 0%;"></div>
                    </div>
                </div>
            </div>
            `;
            nativeSkillsRow.append(skill);
        });
        response.frameworkSkills.forEach(function (e) {
            let skill = `  <div class="col-lg-4 col-md-6 col-12 mb-4 fade-in-hidden">
                <div class="bg-white shadow rounded-3 p-4 d-flex flex-column">
                    <img src="/LogoPictureFiles/${e.logoFileName}" class="rounded-4 mb-3" width="50" height="50" />
                    <h6 class="text-body-secondary mb-1 fw-bolder">${e.title}</h6>
                    <p class="text-body-tertiary mb-4">Below is the percentage of my skill in ${e.title}</p>
                    <span class="text-end d-block color-theme percentage-text">0%</span>
                    <div class="progress" role="progressbar" aria-label="Example 1px high" aria-valuenow="${e.skillPercentage}" aria-valuemin="0" aria-valuemax="100" style="height: 3px">
                        <div class="progress-bar bg-color-theme" style="width: 0%;"></div>
                    </div>
                </div>
            </div>`;
            frameworkSkillsRow.append(skill);
        })

        // Animate the progress bars and the percentage text
        $(".progress-bar").each(function (index) {
            let progressBar = $(this);
            let percentage = parseInt(progressBar.closest(".progress").attr("aria-valuenow"), 10);
            let percentageText = progressBar.closest(".d-flex").find(".percentage-text");

            // Animate progress bar width
            setTimeout(function () {
                progressBar.animate({ width: `${percentage}%` }, 1000);
            }, index * 200);

            // Animate the percentage text count
            let currentCount = 0;
            let intervalDuration = 1000 / percentage; // Divide duration equally for increments
            setTimeout(function () {
                let counter = setInterval(function () {
                    if (currentCount < percentage) {
                        currentCount++;
                        percentageText.text(`${currentCount}%`);
                    } else {
                        clearInterval(counter);
                    }
                }, intervalDuration);
            }, index * 200);
        });
    },
    error: function (xhr, status, error) {
        console.error("Error fetching skills:", error);
    }
});
