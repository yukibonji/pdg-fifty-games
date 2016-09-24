var sounds = {
    table: { "turn": "content/sounds/turn.wav", "death": "content/sounds/death.wav" },
    sounds: {},
    loadSounds: function () {
        for (var key in this.table) {
            var sound = document.createElement("audio");
            sound.src = this.table[key];
            sound.setAttribute("preload", "auto");
            sound.setAttribute("controls", "none");
            sound.display = "none";
            document.body.appendChild(sound);
            this.sounds[key] = sound;
        }
    },
    play:function(key){
        if (this.enabled) {
            this.sounds[key].play();
        }
    },
    enabled:true
};