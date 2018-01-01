var rouletteContainerElement = document.getElementById("roulette-container");

$.getJSON(rouletteDataUrl)
    .done(startRouletteApp)
    .fail(errorInLoading);

function startRouletteApp(rouletteData) {
    var h = hyperapp.h;

    var state = {
        isSpinning: false,
        currentPlace: null,
        isTagConditionOpen: false,
        orSearch: false,
        tagSelections: {},
        targetPlaces: rouletteData.places
    };

    function deriveTargetPlaces(tagSelections, orSearch) {
        var tagIds = Object.keys(tagSelections);
        if (tagIds.length === 0) return rouletteData.places;

        function andFilter(place) {
            return tagIds.every(function (tagId) { return place.tags.indexOf(Number(tagId)) >= 0 });
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

                if (state.targetPlaces.length === 0) return;

                var newState = {
                    isSpinning: true,
                    currentPlace: getRandomPlace(state.targetPlaces)
                };

                setTimeout(function () { actions.rouletteNext(); }, ROULETTE_INTERVAL);
                return newState;
            };
        },
        rouletteNext: function () {
            return function (state, actions) {
                if (!state.isSpinning) return;
                if (state.targetPlaces.length === 0) {
                    // 1件もないならリセット
                    return {
                        isSpinning: false,
                        currentPlace: null
                    };
                }

                setTimeout(function () { actions.rouletteNext(); }, ROULETTE_INTERVAL);
                return { currentPlace: getRandomPlace(state.targetPlaces) };
            };
        },
        toggleTagCondition: function () {
            return function (state) {
                return { isTagConditionOpen: !state.isTagConditionOpen };
            }
        },
        selectAndSearch: function () {
            return function (state) {
                return {
                    orSearch: false,
                    targetPlaces: deriveTargetPlaces(state.tagSelections, false)
                };
            };
        },
        selectOrSearch: function () {
            return function (state) {
                return {
                    orSearch: true,
                    targetPlaces: deriveTargetPlaces(state.tagSelections, true)
                };
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

                return {
                    tagSelections: tagSelections,
                    targetPlaces: deriveTargetPlaces(tagSelections, state.orSearch)
                };
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

        var tagConditionAccordion = null;
        if (rouletteData.tags.length > 0) {
            var tagConditionElement = null;
            if (state.isTagConditionOpen) {
                tagConditionElement = h("div", { class: "active content" },
                    h("div", { id: "tag-cond-container" },
                        h("div", { class: "field" },
                            h("div", { class: "ui radio checkbox" + (state.orSearch ? "" : " checked") },
                                h("input", {
                                    id: "and-radio", class: "hidden", type: "radio",
                                    checked: !state.orSearch,
                                    onchange: function (e) { if (e.target.value) actions.selectAndSearch(); }
                                }),
                                h("label", { for: "and-radio" }, "AND")
                            )
                        ),
                        h("div", { class: "field" },
                            h("div", { class: "ui radio checkbox" + (state.orSearch ? " checked" : "") },
                                h("input", {
                                    id: "or-radio", class: "hidden", type: "radio",
                                    checked: state.orSearch,
                                    onchange: function (e) { if (e.target.value) actions.selectOrSearch(); }
                                }),
                                h("label", { for: "or-radio" }, "OR")
                            )
                        )
                    ),
                    h("div", { class: "ui tag labels" },
                        rouletteData.tags.map(function (tag) {
                            return h("a",
                                {
                                    class: "ui label" + (state.tagSelections[tag.id] ? " teal" : ""),
                                    onclick: function () { actions.toggleTagSelection(tag.id); }
                                },
                                tag.name
                            );
                        })
                    )
                );
            }

            tagConditionAccordion = h("div", { class: "ui accordion" },
                h("div",
                    {
                        class: "title" + (state.isTagConditionOpen ? " active" : ""),
                        onclick: function () { actions.toggleTagCondition(); }
                    },
                    h("i", { class: "dropdown icon" }),
                    " タグで絞る"
                ),
                tagConditionElement
            );
        }

        return h("div", {},
            rouletteResultElement,
            h("button",
                {
                    class: "ui primary toggle button" + (state.isSpinning ? " active" : ""),
                    disabled: state.targetPlaces.length === 0,
                    onclick: function () { actions.toggleSpinning(); },
                },
                state.isSpinning
                    ? [h("i", { class: "hand paper icon" }), " や、これで"]
                    : "まわれーまわれー(ﾟДﾟ)ﾉ"
            ),
            tagConditionAccordion
        );
    }

    hyperapp.app(state, actions, view, document.getElementById("roulette-container"));
}

function errorInLoading() {
    rouletteContainerElement.innerHTML = '<button class="ui disabled button">ルーレットを読み込めませんでした</button>';
}
