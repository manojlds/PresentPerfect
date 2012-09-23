(function () {
    "use strict";

    var list = new WinJS.Binding.List();
    var groupedItems = list.createGrouped(
        function groupKeySelector(item) { return item.group.key; },
        function groupDataSelector(item) { return item.group; }
    );

    loadEvaluations();

    WinJS.Namespace.define("Data", {
        items: groupedItems,
        groups: groupedItems.groups,
        getItemReference: getItemReference,
        getItemsFromGroup: getItemsFromGroup,
        resolveGroupReference: resolveGroupReference,
        resolveItemReference: resolveItemReference
    });

    // Get a reference for an item, using the group key and item title as a
    // unique reference to the item that can be easily serialized.
    function getItemReference(item) {
        return [item.group.key, item.title];
    }

    // This function returns a WinJS.Binding.List containing only the items
    // that belong to the provided group.
    function getItemsFromGroup(group) {
        return list.createFiltered(function (item) { return item.group.key === group.key; });
    }

    // Get the unique group corresponding to the provided group key.
    function resolveGroupReference(key) {
        for (var i = 0; i < groupedItems.groups.length; i++) {
            if (groupedItems.groups.getAt(i).key === key) {
                return groupedItems.groups.getAt(i);
            }
        }
    }

    // Get a unique item from the provided string array, which should contain a
    // group key and an item title.
    function resolveItemReference(reference) {
        for (var i = 0; i < groupedItems.length; i++) {
            var item = groupedItems.getAt(i);
            if (item.group.key === reference[0] && item.title === reference[1]) {
                return item;
            }
        }
    }

    function loadEvaluations() {
        Windows.Storage.KnownFolders.documentsLibrary.getFilesAsync().then(function (files) {
            return files.map(function (file) {
                Windows.Data.Xml.Dom.XmlDocument.loadFromFileAsync(file).then(function (evaluations) {
                    var evaluationGroup = {
                        key: file.name,
                        title: file.displayName,
                        subtitle: file.displayName,
                        backgroundImage: getEvaluationBackground(evaluations),
                        description: file.displayName
                    };
                    return evaluations.selectNodes("//Evaluation").map(function (evaluation) {
                        return getValuationData(evaluationGroup, evaluation);
                    });
                }).then(appendPageContent);
            });
        });
    }

    function getEvaluationBackground(evaluations) {
        return getImageFromString(evaluations.selectSingleNode("//Evaluation[1]").selectSingleNode("ImageBase64String").innerText);
    }

    function getImageFromString(base64String) {
        return "data:image/png;base64," + base64String;
    }

    function getValuationData(group, evaluation) {
        var gestureName = evaluation.selectSingleNode("Name").innerText;
        return {
            group: group,
            title: gestureName,
            subtitle: gestureName,
            description: gestureName,
            content: gestureName,
            backgroundImage: getImageFromString(evaluation.selectSingleNode("ImageBase64String").innerText)
        };
    }

    function appendPageContent(evaluations) {
        evaluations.forEach(function (evaluation) {
            list.push(evaluation);
        });
    }
})();
