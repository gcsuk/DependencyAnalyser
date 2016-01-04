var summary;

$.get("/api/Summary", function (data) {

    if (data != null) {

        summary = data;

        var packageDisplay = "";

        $.each(summary, function (index, value) {

            packageDisplay += "<div><a data-id=\"" + value.Id + "\" class=\"package\">" + value.Name + "</a>";

            if (value.VersionCount > 1) {
                packageDisplay += " <span class=\"versionCount\">(" + value.VersionCount + ")</span>";
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
    $("#projects").empty();

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
    $("#projects").empty();

    var selectedPackage = Number($(e.target).attr("data-package"));
    var selectedVersion = $(e.target).attr("data-version");
    var selectedFramework = $(e.target).attr("data-framework");

    $("#versions a").parent().removeClass("selected");
    $("#versions a[data-version='" + selectedVersion + "'][data-framework='" + selectedFramework + "']").parent().addClass("selected");

    $.each(summary, function (packageIndex, pack) {
        if (pack.Id === selectedPackage) {
            $.each(pack.Versions, function (versionIndex, version) {
                if (version.Version === selectedVersion && version.TargetFramework === selectedFramework) {
                    $.each(version.Components, function(componentIndex, component) {
                        $("#components").append("<div><a data-package=\"" + selectedPackage + "\" data-version=\"" + selectedVersion + "\" data-framework=\"" + selectedFramework + "\" data-component=\"" + component.Name + "\">" + component.Name + "</a> <a href=\"/ComponentDetails/" + component.Id + "\" class=\"pull-right\">[Details]</a></div>");
                    });
                    return false;
                }
            });
            return false;
        }
    });

});

$("#components").on("click", "a", function (e) {

    $("#projects").empty();

    var selectedPackage = Number($(e.target).attr("data-package"));
    var selectedVersion = $(e.target).attr("data-version");
    var selectedFramework = $(e.target).attr("data-framework");
    var selectedComponent = $(e.target).attr("data-component");

    $("#components a").parent().removeClass("selected");
    $("#components a[data-component='" + selectedComponent + "']").parent().addClass("selected");

    $.each(summary, function (packageIndex, pack) {
        if (pack.Id === selectedPackage) {
            $.each(pack.Versions, function (versionIndex, version) {
                if (version.Version === selectedVersion && version.TargetFramework === selectedFramework) {
                    $.each(version.Components, function (componentIndex, component) {
                        if (component.Name === selectedComponent) {
                            $.each(component.Projects, function (projectIndex, project) {
                                $("#projects").append("<div>" + project.Name + "</div>");
                            });
                            return false;
                        }
                    });
                    return false;
                }
            });
            return false;
        }
    });

});