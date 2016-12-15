var game = {
    resetGameData: function(){
        gameData = {};
    },
    loadGameData: function () {
        gameData = JSON.parse(localStorage.gameData || "{}");
    },
    saveGameData: function () {
        localStorage.gameData = JSON.stringify(gameData);
    },
    newGame: function () {
        this.resetGameData();

        //choose  a starting location for player
        var player = {};
        gameData.player = player;

        player.x = Utility.randomIntMax(constants.world.columns());
        player.y = Utility.randomIntMax(constants.world.rows());
        player.z = 0;

        //determine starting atlas column and row
        var atlasColumn = Atlas.worldPositionToAtlasColumn(player.x, player.y);
        var atlasRow = Atlas.worldPositionToAtlasRow(player.y, player.y);

        Atlas.generateMap(atlasColumn, atlasRow, player.z);

        for (var direction = 0; direction < directions.count() ; ++direction) {
            var column = Directions.stepX(atlasColumn, atlasRow, 1);
            var row = Directions.stepY(atlasColumn, atlasRow, 1);
            Atlas.generateMap(column, row, player.z);
        }
    }
};
