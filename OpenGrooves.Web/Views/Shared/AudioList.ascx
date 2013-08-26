<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<AudioModel>>" %>

<%: Html.ScriptInclude("~/content/jplayer/jquery.jplayer.min.js") %>
<%: Html.ScriptInclude("~/content/jplayer/add-on/jplayer.playlist.min.js")%>
<%: Html.CssInclude("~/content/jplayer/blue.monday/jplayer.blue.monday.css") %>

playert start

<div id="jquery_jplayer_1" class="jp-jplayer"></div><div id="jp_container_1" class="jp-audio">	<div class="jp-type-playlist">		<div class="jp-gui jp-interface">			<ul class="jp-controls">				<li><a href="javascript:;" class="jp-previous" tabindex="1">previous</a></li>				<li><a href="javascript:;" class="jp-play" tabindex="1">play</a></li>				<li><a href="javascript:;" class="jp-pause" tabindex="1">pause</a></li>				<li><a href="javascript:;" class="jp-next" tabindex="1">next</a></li>				<li><a href="javascript:;" class="jp-stop" tabindex="1">stop</a></li>				<li><a href="javascript:;" class="jp-mute" tabindex="1" title="mute">mute</a></li>				<li><a href="javascript:;" class="jp-unmute" tabindex="1" title="unmute">unmute</a></li>				<li><a href="javascript:;" class="jp-volume-max" tabindex="1" title="max volume">max volume</a></li>			</ul>
			<div class="jp-progress">				<div class="jp-seek-bar">					<div class="jp-play-bar"></div>				</div>			</div>
			<div class="jp-volume-bar">				<div class="jp-volume-bar-value"></div>			</div>
			<div class="jp-current-time"></div>			<div class="jp-duration"></div>			<ul class="jp-toggles">				<li><a href="javascript:;" class="jp-shuffle" tabindex="1" title="shuffle">shuffle</a></li>				<li><a href="javascript:;" class="jp-shuffle-off" tabindex="1" title="shuffle off">shuffle off</a></li>				<li><a href="javascript:;" class="jp-repeat" tabindex="1" title="repeat">repeat</a></li>				<li><a href="javascript:;" class="jp-repeat-off" tabindex="1" title="repeat off">repeat off</a></li>			</ul>
		</div>
		<div class="jp-playlist">			<ul>				<li></li>			</ul>		</div>
		<div class="jp-no-solution">			<span>Update Required</span>			To play the media you will need to either update your browser to a recent version or update your <a href="http://get.adobe.com/flashplayer/" target="_blank">Flash plugin</a>.		</div>	</div></div><% var jsonPlayList = Model.Select(a => String.Format("{{title: \"{0}\", mp3: \"/uploads/audio/{1}\" }}", a.Title, a.Url)).Join(","); %>

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