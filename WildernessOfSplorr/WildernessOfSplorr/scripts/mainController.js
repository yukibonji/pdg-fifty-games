(function ($) {
    $.QueryString = (function (a) {
        if (a == "") return {};
        var b = {};
        for (var i = 0; i < a.length; ++i) {
            var p = a[i].split('=', 2);
            if (p.length != 2) continue;
            b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
        }
        return b;
    })(window.location.search.substr(1).split('&'))
})(jQuery);
var mainController = {
    imageData: null,
    handleKeyDown: function (event) {
    },
    onInterval: function () {
    },
    renderScreen: function () {
        this.mapRenderer.render();

        frameBuffer.present();
    },
    loadSettings: function () {
        var settings = JSON.parse(localStorage.settings || "{\"soundsEnabled\":true}");
        sounds.enabled = settings.soundsEnabled;
    },
    saveSettings: function(){
        var settings = { soundsEnabled: sounds.enabled };
        localStorage.settings = JSON.stringify(settings);
    },
    initialize: function () {
        this.username = $.QueryString.gjapi_username;
        this.token = $.QueryString.gjapi_token;
        this.loadSettings();

        this.mapRenderer = new Renderer(
            this.context,
            images,
            constants.mapRenderer.offsetX(),
            constants.mapRenderer.offsetY(),
            constants.mapRenderer.cellWidth(),
            constants.mapRenderer.cellHeight());

        this.mapRenderer.columns = constants.mapRenderer.columns();
        this.mapRenderer.rows = constants.mapRenderer.rows();

        this.mapRenderer.clear = function () {
            this.context.fillStyle = "#202020";
            this.context.fillRect(this.offsetX,this.offsetY,this.cellWidth * this.columns, this.cellHeight * this.rows);
        };

        this.mapRenderer.render = function () {
            this.clear();
            for (var x = 0; x < this.columns; ++x) {
                for (var y = 0; y < this.rows; ++y) {
                    if(!(x>=6 && x<=8 && y>=6 && y<=8)){
                        this.drawImage("?", "black", "darkgold", x, y);
                    } else {
                        this.drawImage("field", "darkjade", "black", x, y);
                    }
                }
            }
            this.drawImage("tagon", "medium", "black", 7, 7);
        };

        this.renderScreen();

    },
    context: null
};