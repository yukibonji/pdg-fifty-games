var images = {
    setScale: function (scale) {
        if (scale !== this.scale) {
            this.scale = scale;
            this.canvases = {};
        }
    },
    getImage: function (pattern, foreground, background) {
        if (this.canvases[background] == null) {
            this.canvases[background] = {};
        }
        if (this.canvases[background][foreground] == null) {
            this.canvases[background][foreground] = {};
        }
        if (this.canvases[background][foreground][pattern] == null) {
            var patternData = patterns[pattern];
            if (patternData == null) {
                patternData = { data: [], width: 0 };
            }
            var foregroundColor = colors[foreground];
            if (foregroundColor == null) {
                foregroundColor = [0, 0, 0, 0];
            }
            var backgroundColor = colors[background];
            if (backgroundColor == null) {
                backgroundColor = [0, 0, 0, 0];
            }
            var canvas = document.createElement("canvas");
            this.canvases[background][foreground][pattern] = canvas;
            canvas.width = patternData.width * this.scale;
            canvas.height = patternData.data.length * this.scale;
            canvas.virtualWidth = patternData.width;
            canvas.virtualHeight = patternData.data.length;
            var context = canvas.getContext("2d");
            var imageData = context.createImageData(patternData.width * this.scale, patternData.data.length * this.scale);
            for (var y = 0; y < patternData.data.length; ++y) {
                var data = patternData.data[y];
                for (var x = 0; x < patternData.width; ++x) {
                    var color;
                    if (data % 2 != 0) {
                        color = foregroundColor;
                    } else {
                        color = backgroundColor;
                    }
                    for (var sx = 0; sx < this.scale; ++sx) {
                        for (var sy = 0; sy < this.scale; ++sy) {
                            imageData.data[y * 4 * canvas.width * this.scale + sy * 4 * canvas.width + x * 4 * this.scale + sx * 4 + 0] = color[0];
                            imageData.data[y * 4 * canvas.width * this.scale + sy * 4 * canvas.width + x * 4 * this.scale + sx * 4 + 1] = color[1];
                            imageData.data[y * 4 * canvas.width * this.scale + sy * 4 * canvas.width + x * 4 * this.scale + sx * 4 + 2] = color[2];
                            imageData.data[y * 4 * canvas.width * this.scale + sy * 4 * canvas.width + x * 4 * this.scale + sx * 4 + 3] = color[3];
                        }
                    }
                    data = Math.floor(data / 2);
                }
            }
            context.putImageData(imageData, 0, 0);
        }
        return this.canvases[background][foreground][pattern];
    },
    scale: 1,
    canvases: {}
};