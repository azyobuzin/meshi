var rouletteContainerElement = document.getElementById("roulette-container");

$.getJSON(rouletteDataUrl)
    .done(startRouletteApp)
    .fail(errorInLoading);

function startRouletteApp(rouletteData) {
    var h = hyperapp.h;

    var state = {
        isSpinning: false,
        currentPlace: null,
        orSearch: false,
        tagSelections: {},
        targetPlaces: []
    };

    function deriveTargetPlaces(tagSelections, orSearch) {
        var tagIds = Object.keys(tagSelections);
        if (tagIds.length === 0) return rouletteData.places;

        function andFilter(place) {
            return tagIds.every(function (tagId) { return place.tags.indexOf(tagId) >= 0 });
        }

        function orFilter(place) {
            return place.tags.some(function (tagId) { return !!tagSelections[tagId] });
        }

        return rouletteData.places.filter(orSearch ? orFilter : andFilter);
    }

    function getRandomPlace(targetPlaces) {
        return targetPlaces[Math.floor(Math.random() * targetPlaces.length)];
    }

    var ROULETTE_INTERVAL = 50;

    var actions = {
        toggleSpinning: function () {
            return function (state, actions) {
                if (state.isSpinning) {
                    return { isSpinning: false };
                }

                var targetPlaces = deriveTargetPlaces(state.tagSelections, state.orSearch);
                var newState = {
                    isSpinning: true,
                    targetPlaces: targetPlaces,
                    currentPlace: getRandomPlace(targetPlaces)
                };

                setTimeout(function () { actions.rouletteNext(); }, ROULETTE_INTERVAL);
                return newState;
            };
        },
        toggleTagSelection: function (tagId) {
            return function (state) {
                var tagSelections = {};
                for (var key in state.tagSelections)
                    tagSelections[key] = state.tagSelections[key];

                if (tagSelections[tagId]) {
                    delete tagSelections[tagId];
                } else {
                    tagSelections[tagId] = true;
                }

                var newState = { tagSelections: tagSelections };
                if (state.isSpinning)
                    newState.targetPlaces = deriveTargetPlaces(tagSelections, state.orSearch);
                return newState;
            };
        },
        rouletteNext: function () {
            return function (state, actions) {
                if (!state.isSpinning) return;
                setTimeout(function () { actions.rouletteNext(); }, ROULETTE_INTERVAL);
                return { currentPlace: getRandomPlace(state.targetPlaces) };
            };
        }
    };

    function view(state, actions) {
        var rouletteResultElement = null;
        if (state.currentPlace) {
            rouletteResultElement = h("div", { class: "ui large header" },
                h("a", { href: "../../places/details/" + state.currentPlace.id },
                    state.currentPlace.name
                ),
            );
        }

        var buttonElement = h("button",
            {
                class: "ui primary toggle button" + (state.isSpinning ? " active" : ""),
                onclick: function () { actions.toggleSpinning(); },
            },
            state.isSpinning
                ? [h("i", { class: "hand paper icon" }), " や、これで"]
                : "まわれーまわれー(ﾟДﾟ)ﾉ"
        );

        return h("div", {},
            rouletteResultElement,
            buttonElement
        );

        // TODO: タグ選択 UI
    }

    hyperapp.app(state, actions, view, document.getElementById("roulette-container"));
}

function errorInLoading() {
    rouletteContainerElement.innerHTML = '<button class="ui disabled button">ルーレットを読み込めませんでした</button>';
}
