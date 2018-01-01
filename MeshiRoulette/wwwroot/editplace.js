var tagsDataElement = document.getElementById("tags-data");

var tagsActions = (function () {
    var serverTags = JSON.parse(tagsDataElement.value);
    if (!(serverTags instanceof Array)) serverTags = [];

    var h = hyperapp.h;

    var state = {
        input: "",
        isInputError: false,
        tags: serverTags
    };

    function addTagCore(state) {
        if (!state.input) return {};
        if (state.tags.indexOf(state.input) >= 0) {
            // 重複
            return { isInputError: true };
        }

        return {
            input: "",
            isInputError: false,
            tags: state.tags.concat(state.input)
        };
    }

    var actions = {
        addTag: function () { return addTagCore; },
        removeTag: function (index) {
            return function (state) {
                var newTags = [].concat(state.tags);
                newTags.splice(index, 1);
                return { tags: newTags };
            };
        },
        input: function (value) {
            return function (state) {
                return { input: value, isInputError: false };
            }
        },
        beforeSubmit: function () {
            return function (state) {
                var newState = addTagCore(state);
                tags = newState.tags;
                if (typeof tags === "undefined") tags = state.tags;
                tagsDataElement.value = JSON.stringify(tags);
                return newState;
            }
        }
    };

    function view(state, actions) {
        var tagElements = state.tags.map(function (tag, index) {
            return h("a", { class: "ui label", onclick: function () { actions.removeTag(index); } }, [
                tag + " ",
                h("i", { class: "close icon" })
            ]);
        });

        return h("div", { class: "" /* loader を消す */ },
            h("div", { class: "field" + (state.isInputError ? " error" : "") },
                h("label", { for: "tag-input" }, "タグ"),
                h("div", { class: "ui right labeled input" },
                    h("input", {
                        id: "tag-input",
                        type: "text",
                        placeholder: "新しいタグ名",
                        value: state.input,
                        oninput: function (e) { actions.input(e.target.value); },
                        onkeypress: function (e) {
                            // Enter を拾って submit を阻止
                            if (e.keyCode === 13) {
                                e.preventDefault();
                                actions.addTag();
                            }
                        }
                    }),
                    h("button", { class: "ui tag label", type: "button", onclick: function () { actions.addTag(); } }, "追加")
                )
            ),
            h("div", { id: "tags-container", class: "ui tag labels" }, tagElements)
        );
    }

    return hyperapp.app(state, actions, view, document.getElementById("tags-view"));
})();

document.getElementById("edit-place-form").addEventListener(
    "submit",
    function () { tagsActions.beforeSubmit(); },
    false
);
