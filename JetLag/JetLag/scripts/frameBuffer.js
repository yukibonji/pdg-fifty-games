var frameBuffer = {
    "initialize": function (displayCanvas, virtualWidth, virtualHeight) {
        this.displayCanvas = displayCanvas;
        this.displayContext = displayCanvas.getContext("2d");

        this.virtualWidth = virtualWidth;
        this.virtualHeight = virtualHeight;

        var screenWidth = screen.width;
        var screenHeight = screen.height;

        var viewXScale = screenWidth / this.virtualWidth;
        var viewYScale = screenHeight / this.virtualHeight;

        var viewScale = Math.ceil(Math.max(viewXScale, viewYScale));
        images.setScale(viewScale);

        this.backBufferWidth = this.virtualWidth * viewScale;
        this.backBufferHeight = this.virtualHeight * viewScale;

        this.backBufferCanvas = document.createElement("canvas");
        this.backBufferCanvas.width = this.backBufferWidth;
        this.backBufferCanvas.height = this.backBufferHeight;

        this.backBufferContext = this.backBufferCanvas.getContext("2d");
        this.backBufferContext.scale(viewScale, viewScale);
    },
    "fitToWindow": function () {
        var windowWidth = window.innerWidth;
        var windowHeight = window.innerHeight;

        this.displayCanvas.width = windowWidth;
        this.displayCanvas.height = windowHeight;

        this.displayContext.fillStyle = "#100010";
        this.displayContext.fillRect(0, 0, windowWidth, windowHeight);

        var windowXScale = windowWidth / this.backBufferWidth;
        var windowYScale = windowHeight / this.backBufferHeight;

        var windowScale = Math.min(windowXScale, windowYScale);

        this.displayWidth = this.backBufferWidth * windowScale;
        this.displayHeight = this.backBufferHeight * windowScale;

        this.displayX = windowWidth / 2 - this.displayWidth / 2;
        this.displayY = windowHeight / 2 - this.displayHeight / 2;
    },
    "present": function () {
        this.displayContext.drawImage(this.backBufferCanvas, this.displayX, this.displayY, this.displayWidth, this.displayHeight);
    }
};