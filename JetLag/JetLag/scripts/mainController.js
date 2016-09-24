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
    loadHighScore: function () {
        if (localStorage == null || localStorage.highScore == null) {
            return 0;
        } else {
            return Number(localStorage.highScore);
        }
    },
    saveHighScore: function () {
        localStorage.highScore = this.highScore;
    },
    handleKeyDown: function (event) {
        if (this.gameOver) {
            if (event.which == 32) {
                this.setUpGame();
                this.gameOver = false;
                this.interval = setInterval(function () {
                    mainController.onInterval();
                }, this.ticksPerFrame);
            } else if (event.which == 77) {
                sounds.enabled = !sounds.enabled;
                this.saveSettings();
                this.renderScreen();
            }
        } else {
            if (event.which == 37 && this.direction != -1) {
                sounds.play("turn");
                this.score += this.runLength * (this.runLength + 1) / 2;
                if (this.score > this.highScore) {
                    this.highScore = this.score;
                    this.saveHighScore();
                }
                this.runLength = 0;
                this.direction = -1;
            } else if (event.which == 39 && this.direction != 1) {
                sounds.play("turn");
                this.score += this.runLength * (this.runLength + 1) / 2;
                if (this.score > this.highScore) {
                    this.highScore = this.score;
                    this.saveHighScore();
                }
                this.runLength = 0;
                this.direction = 1;
            }
        }
    },
    onInterval: function () {
        for (var y = 0; y < this.rows - 1; ++y) {
            this.blocks[y] = this.blocks[y + 1];
        }
        this.blocks[this.rows - 1] = Math.floor(Math.random() * (this.columns - 2)) + 1;
        for (var y = 0; y < this.tailLength - 1; ++y) {
            this.tail[y] = this.tail[y + 1];
        }
        this.tail[this.tailLength - 1] += this.direction;
        if (this.tail[this.tailLength - 1] == this.blocks[this.tailLength - 1] || this.tail[this.tailLength - 1] == 0 || this.tail[this.tailLength - 1]==this.columns-1) {
            sounds.play("death");
            this.gameOver = true;
            if (this.username != null && this.token != null) {
                GJAPI.sendURL(GJAPI.getURL("scores/add", { username: this.username, user_token: this.token, score: this.highScore, sort: this.highScore, table_id: 197668 }), function (data) { });
            }
            clearInterval(this.interval);
        } else {
            this.runLength++;
        }
        this.renderScreen();
    },
    drawImage:function (patternName,foreground,background,x,y){
        image = images.getImage(patternName, foreground, background);
        this.context.drawImage(image, x * this.cellWidth, y * this.cellHeight, this.cellWidth, this.cellHeight);
    },
    drawText: function (text, foreground, background, x, y) {
        for (var index = 0; index < text.length; ++index) {
            this.drawImage(text.charAt(index), foreground, background, x, y);
            x++;
        }
    },
    setUpGame: function () {
        this.blocks = [];
        while (this.blocks.length < this.rows) {
            this.blocks.push(0);
        }
        this.tail = [];
        while (this.tail.length < this.tailLength) {
            this.tail.push(Math.floor(this.columns / 2));
        }
        this.score = 0;
        this.direction = 1;
        this.runLength = 0;
    },
    renderScreen: function () {
        this.context.fillStyle = "#202020";
        this.context.fillRect(0, 0, this.columns * this.cellWidth, this.rows * this.cellHeight);

        var image = images.getImage("\u00db", "silver", "silver");
        for (var y = 0; y < this.rows; ++y) {
            this.drawImage("\u00db", "silver", "silver", this.blocks[y], y);
        }

        image = images.getImage("\u00db", "ruby", "ruby");
        for (var y = 0; y < this.rows; ++y) {
            this.drawImage("\u00db", "ruby", "ruby", 0, y);
            this.drawImage("\u00db", "ruby", "ruby", this.columns - 1, y);
        }

        image = images.getImage("*", "copper", "transparent");
        for (var y = 0; y < this.tailLength - 1; ++y) {
            this.drawImage("*", "copper", "transparent", this.tail[y], y);
        }

        this.drawImage("\u0002", "turquoise", "transparent", this.tail[this.tailLength - 1], this.tailLength - 1);

        if (this.gameOver) {
            var text = "Press [SPACE] to Start";
            this.drawText(text, "amethyst", "transparent", this.columns / 2 - text.length / 2, this.rows / 2 - 1);

            text = "Controls: \u001b \u001a";
            this.drawText(text, "mediumamethyst", "transparent", this.columns / 2 - text.length / 2, this.rows / 2 + 1);
        }

        var s = String(this.score);
        this.drawText(s, "jade", "transparent", 1, 0);

        s = String(this.highScore);
        this.drawText(s, "jade", "transparent", this.columns - 1 - s.length, 0);

        if (this.gameOver) {
            var text = (sounds.enabled) ? "[M]ute" : "Un[M]ute";
            this.drawText(text, "gold", "transparent", this.columns / 2 - text.length / 2, this.rows - 1);
        }

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
        this.username = $.QueryString.username;
        this.token = $.QueryString.token;
        this.loadSettings();
        this.context.fillStyle = "#202020";
        this.context.fillRect(0, 0, frameBuffer.virtualWidth, frameBuffer.virtualHeight);

        this.columns = 40;
        this.rows = 30;
        this.cellWidth = 8;
        this.cellHeight = 8;
        this.tailLength = 6;
        this.highScore = this.loadHighScore();
        this.gameOver = true;
        this.ticksPerFrame = 100;

        this.setUpGame();

        this.renderScreen();

    },
    interval: null,
    context: null
};