﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<AudioModel>>" %>

<%: Html.ScriptInclude("~/content/jplayer/jquery.jplayer.min.js") %>
<%: Html.ScriptInclude("~/content/jplayer/add-on/jplayer.playlist.min.js")%>
<%: Html.CssInclude("~/content/jplayer/blue.monday/jplayer.blue.monday.css") %>

playert start

<div id="jquery_jplayer_1" class="jp-jplayer"></div>
			<div class="jp-progress">
			<div class="jp-volume-bar">
			<div class="jp-current-time"></div>
		</div>
		<div class="jp-playlist">
		<div class="jp-no-solution">

<script type="text/javascript">

    new jPlayerPlaylist({
        jPlayer: "#jquery_jplayer_1",
        cssSelectorAncestor: "#jp_container_1"
    }, [<%= jsonPlayList %>], {
	    swfPath: "/content/jplayer",
	    supplied: "mp3",
	    wmode: "window"
	});
</script>