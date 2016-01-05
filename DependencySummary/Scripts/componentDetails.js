var packages;

$.get("/api/Summary?componentId=" + $("#id").val(), function (data) {

    if (data != null) {

        packages = data.Packages;

        var packageDisplay = "";

        $.each(packages, function (index, value) {
            packageDisplay += "<div><a data-package=\"" + value.Name + "\" data-version=\"" + value.Version + "\" data-framework=\"" + value.TargetFramework + "\" class=\"package\">" + value.Name + " - " + value.Version + " - " + value.TargetFramework + "</a></div>";
        });

        $("#packages").html(packageDisplay);

        $("#loading").hide();
        $("#content").removeClass("hidden");
    }

});

$("#packages").on("click", "a", function (e) {

    $("#projects").empty();

    var selectedPackage = $(e.target).attr("data-package");
    var selectedVersion = $(e.target).attr("data-version");
    var selectedFramework = $(e.target).attr("data-framework");

    $("#packages a").parent().removeClass("selected");
    $("#packages a[data-package='" + selectedPackage + "'][data-version='" + selectedVersion + "'][data-framework='" + selectedFramework + "']").parent().addClass("selected");

    $.each(packages, function (packageIndex, pack) {
        if (pack.Name === selectedPackage) {
            $.each(pack.Projects, function (projectIndex, project) {
                $("#projects").append("<div>" + project.Name + "</div>");
            });
            return false;
        }
    });

    window.scrollTo(0, 0);

});

$("#componentList").change(function () {
    window.location = "/ComponentDetails/" + $(this).val();
});