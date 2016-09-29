function Renderer(context, images, offsetX, offsetY, cellWidth, cellHeight) {
    this.images = images;
    this.context = context;
    this.offsetX = offsetX;
    this.offsetY = offsetY;
    this.cellWidth = cellWidth;
    this.cellHeight = cellHeight;

    this.context = context;

    this.drawImage = function (patternName, foreground, background, x, y) {
        var image = this.images.getImage(patternName, foreground, background);
        this.context.drawImage(image, this.offsetX + x * this.cellWidth, this.offsetY + y * this.cellHeight, this.cellWidth, this.cellHeight);
    };
    this.drawText = function (text,foreground,background,x,y) {
        for (var index = 0; index < text.length; ++index) {
            this.drawImage(text.charAt(index), foreground, background, x, y);
            x++;
        }
    };

}