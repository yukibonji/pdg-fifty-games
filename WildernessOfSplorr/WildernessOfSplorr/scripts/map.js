var Map = {
    worldPositionToMapColumn: function (worldColumn, worldRow) {
        var result = (Math.floor(worldColumn) % constants.map.columns());
        return (result >= 0) ? (result) : (result + constants.map.columns());
    },
    worldPositionToMapRow: function (worldColumn, worldRow) {
        var result = (Math.floor(worldRow) % constants.map.rows());
        return (result >= 0) ? (result) : (result + constants.map.rows());
    }
};