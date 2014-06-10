<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="noNodes.aspx.cs" Inherits="Concorde.CacheHandler.noNodes" %>
<%@ Import Namespace="Concorde.CacheHandler.Managers" %>
<%@ Import Namespace="Umbraco.Core.IO" %>
<!doctype html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Umbraco - no pages found</title>
    <link rel="icon" type="image/png" href="<%=umbraco.GlobalSettings.Path + "/Images/PinnedIcons/umb.ico" %>" />
    <link rel="stylesheet" href="../../umbraco/assets/css/installer.css" />
    <style type="text/css">
        @-webkit-keyframes opacity {
            0% {
                opacity: 1;
            }

            100% {
                opacity: 0;
            }
        }

        @-moz-keyframes opacity {
            0% {
                opacity: 1;
            }

            100% {
                opacity: 0;
            }
        }

        #loading span {
            -webkit-animation-name: opacity;
            -webkit-animation-duration: 1s;
            -webkit-animation-iteration-count: infinite;
            -moz-animation-name: opacity;
            -moz-animation-duration: 1s;
            -moz-animation-iteration-count: infinite;
        }

            #loading span:nth-child(2) {
                -webkit-animation-delay: 100ms;
                -moz-animation-delay: 100ms;
            }

            #loading span:nth-child(3) {
                -webkit-animation-delay: 300ms;
                -moz-animation-delay: 300ms;
            }
        <%= ActivationManager.EventFeedback.IsDatabaseCreated || ActivationManager.EventFeedback.HasItemsToProcess ? "" : "#installer {height: 800px;}" %>

        /* links */
        .btn-web {
            width: 100%;
            overflow: hidden;
            margin: 0;
            padding: 15px 0 20px;
            list-style: none;
        }

            .btn-web li {
                float: left;
                padding: 0 13px 0 0;
            }

                .btn-web li a {
                    float: left;
                    height: 42px;
                    overflow: hidden;
                    cursor: pointer;
                }

                    .btn-web li a:hover {
                        border: none;
                    }

                    .btn-web li a span {
                        float: left;
                        height: 84px;
                        overflow: hidden;
                        text-indent: -99999px;
                    }

                .btn-web li.btn-set a span {
                    width: 248px;
                    background: url(../../umbraco_client/installer/images/btn-set.png) no-repeat;
                }

                .btn-web li.btn-preview-web a span {
                    width: 257px;
                    background: url(../../umbraco_client/installerimages/btn-preview-web.png) no-repeat;
                }

                .btn-web li a:hover span {
                    margin: -42px 0 0;
                }

        .links {
            width: 100%;
            overflow: hidden;
        }

            .links ul {
                width: 100%;
                overflow: hidden;
                margin: 0;
                padding: 6px 0 14px;
                list-style: none;
            }

                .links ul li {
                    overflow: hidden;
                    height: 1%;
                    vertical-align: top;
                    padding: 0 0 9px 11px;
                    font-size: 14px;
                    line-height: 16px;
                    background: url(../../umbraco_client/installer/images/bul4.gif) no-repeat 0 5px;
                }

                    .links ul li a {
                        text-decoration: underline;
                    }

                        .links ul li a:hover {
                            text-decoration: none;
                        }

        ul.summary {
            margin: 0;
            padding: 0 10px 0 0;
        }

        .summary li {
            margin-bottom: 20px;
            display: block;
        }

        .summary a {
            display: block;
            font-weight: bold;
        }

        /* three columns */
        .threcol {
            width: 100%;
            overflow: hidden;
        }

            .threcol .t {
                width: 100%;
                overflow: hidden;
                height: 1px;
                font-size: 0;
                line-height: 0;
                background: url(../../umbraco_client/installer/images/sep1.png) repeat-x;
            }

            .threcol .hold {
                width: 100%;
                overflow: hidden;
            }

        .content .threcol h2 {
            margin: 0 0 11px;
            font-size: 20px;
            line-height: 24px;
            font-weight: bold;
            color: #fff;
        }

        .col1 {
            float: left;
            width: 200px;
            padding: 0 10px 0 0;
        }

        .col2 {
            float: left;
            width: 250px;
            padding: 0 10px 0 0;
        }

        .col3 {
            float: left;
            width: 250px;
        }

        .content .threcol p {
            margin: 0 0 20px;
            line-height: 20px;
        }

            .content .threcol p span {
                display: block;
            }

            .content .threcol p a {
                color: #fff;
                text-decoration: underline;
            }

                .content .threcol p a:hover {
                    text-decoration: none;
                }
    </style>
    <script src="../../umbraco/lib/jquery/jquery-2.0.3.min.js"></script>
    <script type="text/javascript">
        $(function () {
            //Function to determine how to show the noNodes.aspx page with either feedback from processing Courier items
            //or a more common 'no nodes' type page.
            (function determineFeedbackApproach() {
                $.ajax({
                    url: "/config/splashes/noNodes.aspx",
                    type: "POST",
                    accepts: "application/json",
                    cache: false,
                    headers: {
                        "X-Component": "<%= ClientID %>",
                        "X-Event": "courier-initiate"
                    },
                    success: function (data) {
                        if (data) {
                            if (data.HasItemsToProcess) {
                                poll();
                            } else if (data.HasConfiguredDatabase) {
                                noNodesUpdate();
                            } else {
                                noNodesStandard();
                            }
                        }
                    }
                });
            })();

            //Long Polling function
            function poll() {
                $.ajax({
                    url: "/config/splashes/noNodes.aspx",
                    type: "POST",
                    accepts: "application/json",
                    cache: false,
                    headers: {
                        "X-Component": "<%= ClientID %>",
                        "X-Event": "courier-update"
                    },
                    success: function (data, textStatus, jqXhr) {
                        //Update progress - textStatus: timeout, error, notmodified, success, parsererror
                        updateProgress(data);
                    },
                    statusCode: {
                        404: function () {
                            console.log("404 Not Found");
                        },
                        304: function () {
                            console.log("304 Not Modified");
                        },
                        200: function () {
                            console.log("200 OK");
                        }
                    },
                    dataType: "json",
                    complete: function (jqXhr, textStatus) {
                        // Handle the complete event
                        if (textStatus === "success") {
                            poll(); //Continue polling
                        } else {
                            updateSummary(); //Exit polling
                        }
                    },
                    timeout: 30000
                });
            }

            //Update Progress (continuously) based on server feedback when processing
            function updateProgress(data) {
                if (data) {
                    if (data.Update) {
                        $("#installer .row .span12 .log-updates").append("<div>" + data.Update + "</div>");
                        $('#installer .row .span12 .log-updates').scrollTop($('#installer .row .span12 .log-updates')[0].scrollHeight);
                        $("#installer .row .span12 .log-summary").html("<div>Items Extracted: " + data.ItemsToExtractCurrent + "/" + data.ItemsToExtractTotal + ", Items Processed: " + data.ItemsToProcessCurrent + "/" + data.ItemsToProcessTotal + "</div>").fadeIn("slow");
                        $(".umb-loader-container").width(data.PercentComplete + "%");

                        if (data.ItemsToExtractCurrent === data.ItemsToExtractTotal && data.ItemsToProcessTotal > 0) {
                            if (data.ItemsToProcessCurrent === data.ItemsToProcessTotal) {
                                $("#installer .row .span12 .log-header strong").text("Data is being published");
                            } else {
                                $("#installer .row .span12 .log-header strong").text("Data is being processed");
                            }
                        } else {
                            $("#installer .row .span12 .log-header strong").text("Data is being added");
                        }
                    }
                }
            }

            //Update Summary when processing is done
            function updateSummary() {
                $(".umb-loader-container").addClass("umb-loader-done");
                $("#loading span").remove();
                $("#installer .row .span12 .log-header strong").text("Data has been added").fadeIn("slow");
                $("#installer .row .span12 .log-summary").html("");
                $("#installer .row .span12 .log-summary").append("<strong>Everthing is now ready</strong>").fadeIn("slow");

                $.ajax({
                    url: "/config/splashes/noNodes.aspx",
                    type: "POST",
                    accepts: "application/json",
                    cache: false,
                    headers: {
                        "X-Component": "<%= ClientID %>",
                        "X-Event": "courier-summary"
                    },
                    success: function (data) {
                        if (data) {
                            $("#installer .row .span12 .log-summary").append("<div>Items Extracted: " + data.ItemsExtracted + "/" + data.ItemsToExtractTotal + ", Items Processed: " + data.ItemsProcessed + "/" + data.ItemsToProcessTotal + "</div>").fadeIn("slow");
                            $("#installer .row .span12 .log-summary").append('<p><a href="/umbraco/"> >> Go to the Umbraco backoffice << </a></p>').fadeIn("slow");
                            finishUpdate();
                        }
                    }
                });
            }

            //No Nodes Update when there is nothing to process, but database + user has been created
            function noNodesUpdate() {
                $("#installer .row .span12 .log-header").html("<strong>There is no data to process</strong>").fadeIn("slow");
                $("#installer .row .span12 .log-updates").append("<div><span>[ ]</span> Sql CE database created</div>").fadeIn("slow");
                $("#installer .row .span12 .log-updates").append("<div><span>[ ]</span> Database configured for Umbraco</div>").fadeIn("slow");
                $("#installer .row .span12 .log-updates").append("<div><span>[ ]</span> Umbraco database schema installed</div>").fadeIn("slow");
                $("#installer .row .span12 .log-updates").append("<div><span>[ ]</span> Umbraco backoffice user created</div>").fadeIn("slow");
                $("#installer .row .span12 .log-summary").html("");

                finishUpdate();

                $("#installer .row .span12 .log-updates div span").each(function (i, el) {
                    setTimeout(function () {
                        $(el).text("[x]");
                    }, 800 + (i * 500));
                });

                $("#installer .row .span12 .log-summary").append("<strong>Everthing is now ready</strong>").hide().fadeIn(3000);
                $("#installer .row .span12 .log-summary").append('<p><a href="/umbraco/"> >> Go to the Umbraco backoffice << </a></p>').hide().fadeIn(3000);
            }

            function finishUpdate() {
                $.ajax({
                    url: "/config/splashes/noNodes.aspx",
                    type: "POST",
                    accepts: "application/json",
                    cache: false,
                    headers: {
                        "X-Component": "<%= ClientID %>",
                        "X-Event": "courier-reset"
                    },
                    success: function (data) { }
                });
            }

            //No Nodes standard page when there is not to process and database is already available
            function noNodesStandard() {
                $("#loading").remove();
                $("#installer .row .span12 .log-updates").remove();
                $("#installer .row .span12 .log-summary").remove();
                $("#installer h1").text("Looks like there's still work to do");
                $("#no-nodes").show();

                $.post("../../install/InstallerRestService.aspx?feed=sitebuildervids",
                    function (data) {
                        $("#ajax-sitebuildervids").html(data);
                    });

                $.post("../../install/InstallerRestService.aspx?feed=developervids",
                    function (data) {
                        $("#ajax-developervids").html(data);
                    });
            }
        });
    </script>
</head>
<body id="umbracoInstallPageBody">
    <img src="../../umbraco/assets/img/application/logo_white.png" id="logo" />

    <div class="umb-loader-container">
        <div class="umb-loader" id="loader"></div>
    </div>

    <div id="installer" class="absolute-center clearfix">
        <div>
            <h1>Your local Umbraco site is being configured</h1>
            <div class="row">
                <div class="span12">
                    <div id="loading" class="log-header"><strong>Data is being added</strong> <span>.</span><span>.</span><span>.</span></div>
                    <div class="log-updates" style="height: 440px; overflow: auto; margin-top: 4px; margin-bottom: 4px;"></div>
                    <div class="log-summary"></div>

                    <div id="no-nodes" class="tab main-tabinfo" style="display: none;">
                        <div class="container">
                            <p>
                                You're seeing the wonderful page because your website doesn't contain any <strong>published</strong> content yet.
                            </p>
                            <p>
                                So get rid of this page by starting umbraco and publishing some content. You can do this by clicking the "set up your new website" button below.
                            </p>
                            <ul class="btn-web">
                                <li class="btn-set"><a href="<%= IOHelper.ResolveUrl(SystemDirectories.Umbraco) %>"><span>Launch umbraco</span></a></li>
                            </ul>
                        </div>
                        <div class="threcol">
                            <div class="t">
                                &nbsp;
                            </div>
                            <div class="hold">
                                <aside class="col1">
                                    <h2>Useful links</h2>
                                    <p>We’ve put together some useful links to help you get started with Umbraco.</p>
                                    <nav class="links">
                                        <ul>
                                            <li><a href="http://our.umbraco.org?ref=ourFromInstaller">our.umbraco.org</a></li>
                                        </ul>

                                        <ul>
                                            <li><a href="http://our.umbraco.org/wiki?ref=LatestDocsFromInstaller">New documentation</a></li>
                                            <li><a href="http://our.umbraco.org/projects?ref=LatestProjectsFromInstaller">New Projects</a></li>
                                            <li><a href="http://our.umbraco.org/forum?ref=LatesTalkFromInstaller">Forum Talk</a></li>
                                        </ul>
                                    </nav>
                                </aside>
                                <aside class="col2">
                                    <h2>Sitebuilder introduction</h2>
                                    <div id="ajax-sitebuildervids"><small>Loading...</small></div>
                                </aside>
                                <aside class="col3">
                                    <h2>Developer introduction</h2>
                                    <div id="ajax-developervids"><small>Loading...</small></div>
                                </aside>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
