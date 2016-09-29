function resizeCanvas() {
    frameBuffer.fitToWindow();
    frameBuffer.present();
}

function handleKeyDown(event) {
    mainController.handleKeyDown(event);
}

function main() {
    sounds.loadSounds();
    frameBuffer.initialize(document.getElementById("mainCanvas"), 160, 120);
    frameBuffer.fitToWindow();

    mainController.context = frameBuffer.backBufferContext;
    mainController.initialize();
}