var Atlas = {
    worldPositionToAtlasColumn: function (worldColumn, worldRow) {
        return this.normalizeColumn(Math.floor(worldColumn / constants.map.columns()));
    },
    worldPositionToAtlasRow: function (worldColumn, worldRow) {
        return this.normalizeRow(Math.floor(worldRow / constants.map.rows()));
    },
    normalizeColumn: function (column) {
        var result = column % constants.atlas.columns();
        return (result >= 0) ? (result) : (result + constants.atlas.columns());
    },
    normalizeRow: function (row) {
        var result = row % constants.atlas.rows();
        return (result >= 0) ? (result) : (result + constants.atlas.rows());
    },
    generateMap: function (column, row, layer) {
        column = this.normalizeColumn(column);
        row = this.normalizeRow(row);
        layer = layer || 0;

        if (gameData == null) {
            gameData = {};
        }
        if (gameData.world == null) {
            gameData.world = {};
        }
        if (gameData.world[column] == null) {
            gameData.world[column] = {};
        }
        if (gameData.world[column][row] == null) {
            gameData.world[column][row] = {};
        }
        var map = gameData.world[column][row][layer];
        if (map == null) {
            map = {};
            gameData.world[column][row][layer] = map;
            for (var mapColumn = 0; mapColumn < constants.map.columns() ; ++mapColumn) {
                map[mapColumn] = {};
                for (var mapRow = 0; mapRow < constants.map.rows() ; ++mapRow) {
                    var cell = {};
                    map[mapColumn][mapRow] = cell;
                    cell.terrain = {
                        identifier:"field"
                    };
                    cell.effects = [{identifier:"unexplored"}];
                }
            }
        }

    }
};