var summary;

$.get("/api/Summary", function (data) {

    if (data != null) {

        summary = data;

        var packageDisplay = "";

        $.each(summary, function (index, value) {

            packageDisplay += "<div><a data-id=\"" + value.Id + "\" class=\"package\">" + value.Name + "</a>";

            if (value.VersionCount > 1) {
                packageDisplay += " <span class=\"versionCount\">(" + value.VersionCount + " Versions)</span>";
            }

            packageDisplay += "</div>";

            $("#packages").html(packageDisplay);

        });

        $("#loading").hide();
        $("#content").removeClass("hidden");
    }

});

$("#packages").on("click", "a", function (e) {

    $("#versions").empty();
    $("#components").empty();

    var packageId = Number($(e.target).attr("data-id"));

    $("#packages a").parent().removeClass("selected");
    $("#packages a[data-id=" + packageId + "]").parent().addClass("selected");

    $.each(summary, function (packageIndex, pack) {
        if (pack.Id === packageId) {
            $.each(pack.Versions, function (versionIndex, version) {
                $("#versions").append("<div><a data-package=\"" + packageId + "\" data-version=\"" + version.Version + "\" data-framework=\"" + version.TargetFramework + "\">" + version.Version + " - " + version.TargetFramework + "</a></div>");
            });
            return false;
        }
    });

    window.scrollTo(0, 0);

});

$("#versions").on("click", "a", function (e) {

    $("#components").empty();

    var selectedPackage = Number($(e.target).attr("data-package"));
    var selectedVersion = $(e.target).attr("data-version");
    var selectedFramework = $(e.target).attr("data-framework");

    $("#versions a").parent().removeClass("selected");
    $("#versions a[data-version='" + selectedVersion + "'][data-framework='" + selectedFramework + "']").parent().addClass("selected");

    $.each(summary, function (packageIndex, pack) {
        if (pack.Id === selectedPackage) {
            $.each(pack.Versions, function (versionIndex, version) {
                if (version.Version === selectedVersion) {
                    $.each(version.Components, function(componentIndex, component) {
                        $("#components").append("<div>" + component.Name + "</div>");
                    });
                    return false;
                }
            });
            return false;
        }
    });

});