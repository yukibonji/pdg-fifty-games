var Directions = {
    deltaXs: [0, 1, 1, 1, 0, -1, -1, -1],
    deltaYs: [-1, -1, 0, 1, 1, 1, 0, -1],
    opposites: [4, 5, 6, 7, 0, 1, 2, 3],
    nexts: [1, 2, 3, 4, 5, 6, 7, 0],
    previouses: [7, 0, 1, 2, 3, 4, 5, 6],
    count: function () { return (this.deltaXs.length); },
    normalize: function (direction) {
        var result = direction % this.count();
        return (direction >= 0) ? (direction) : (direction + this.count());
    },
    stepX: function (direction, x, y, steps) {
        steps = steps || 1;
        direction = this.normalize(direction);
        if (steps < 0) {
            return this.stepX(this.opposite(direction), x, y, -steps);
        } else if (steps == 0) {
            return x;
        } else {
            x += this.deltaXs[direction];
            y += this.deltaYs[direction];
            steps--;
            return this.stepX(direction, x, y, steps);
        }
    },
    stepY: function (direction, x, y, steps) {
        steps = steps || 1;
        direction = this.normalize(direction);
        if (steps < 0) {
            return this.stepY(this.opposite(direction), x, y, -steps);
        } else if (steps == 0) {
            return x;
        } else {
            x += this.deltaXs[direction];
            y += this.deltaYs[direction];
            steps--;
            return this.stepY(direction, x, y, steps);
        }
    },
    next: function (direction) {
        return this.nexts[this.normalize(direction)];
    },
    previous: function (direction) {
        return this.previouses[this.normalize(direction)];
    },
    opposite: function (direction) {
        return this.opposites[this.normalize(direction)];
    }
};