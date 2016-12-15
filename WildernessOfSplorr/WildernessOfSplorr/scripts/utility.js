var Utility = {
    randomIntMax: function (max) {
        return Math.floor(Math.random() * max);
    },
    normalize: function (value, max) {
        var result = column % max;
        return (result >= 0) ? (result) : (result + max);
    }
};