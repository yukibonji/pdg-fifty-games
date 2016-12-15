var constants = {
    patterns: {
        width: function () { return (8); },
        height: function () { return (8); }
    },
    mapRenderer : {
        offsetX : function(){return (0);},
        offsetY : function(){return (0);},
        cellWidth : function(){return constants.patterns.width();},
        cellHeight: function () { return constants.patterns.height(); },
        columns: function () { return constants.map.columns(); },
        rows: function () { return constants.map.rows(); },
    },
    map: {
        columns: function () { return (15); },
        rows: function () { return (15); }
    },
    atlas: {
        columns: function () { return (256); },
        rows: function () { return (256); },
    },
    world: {
        columns: function () { return constants.atlas.columns() * constants.map.columns(); },
        rows: function () { return constants.atlas.rows() * constants.map.rows(); }
    }

};