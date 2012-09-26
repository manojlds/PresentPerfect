var Evaluation = {
    initalized: false,
    init: function () {
        if (this.initalized) return this.self;
        this.self = this;
        this.list = new WinJS.Binding.List();
        this.groupedItems = this.list.createGrouped(
            function groupKeySelector(item) { return item.group.key; },
            function groupDataSelector(item) { return item.group; }
        );
        WinJS.Namespace.define("Data", {
            items: this.self.groupedItems,
            groups: this.self.groupedItems.groups,
            getItemReference: this.self.getItemReference,
            getItemsFromGroup: this.self.getItemsFromGroup.bind(this.self),
            resolveGroupReference: this.self.resolveGroupReference.bind(this.self),
            resolveItemReference: this.self.resolveItemReference
        });
        this.initalized = true;
        return this.self;
    },
    getItemReference: function (item) {
        return [item.group.key, item.title];
    },
    getItemsFromGroup: function (group) {
        return this.list.createFiltered(function (item) { return item.group.key === group.key; });
    },
    resolveGroupReference: function (key) {
        for (var i = 0; i < this.groupedItems.groups.length; i++) {
            if (this.groupedItems.groups.getAt(i).key === key) {
                return this.groupedItems.groups.getAt(i);
            }
        }
        return null;
    },
    resolveItemReference: function (reference) {
        for (var i = 0; i < this.groupedItems.length; i++) {
            var item = this.groupedItems.getAt(i);
            if (item.group.key === reference[0] && item.title === reference[1]) {
                return item;
            }
        }
        return null;
    },
    loadEvaluations: function () {
        Windows.Storage.KnownFolders.documentsLibrary.getFilesAsync().then(function (files) {

            return files.map(this.loadContentFromFile.bind(this));
        }.bind(this));
    },
    loadContentFromFile: function (file) {
        Windows.Data.Xml.Dom.XmlDocument.loadFromFileAsync(file).then(function (evaluations) {
            var evaluationGroup = {
                key: file.name,
                title: file.displayName,
                subtitle: file.displayName,
                backgroundImage: this.getEvaluationBackground(evaluations),
                description: file.displayName
            };
            return evaluations.selectNodes("//Evaluation").map(function (evaluation) {
                return this.getValuationData(evaluationGroup, evaluation);
            }.bind(this));
        }.bind(this)).then(this.appendPageContent.bind(this));
    },
    getEvaluationBackground: function (evaluations) {
        return this.getImageFromString(evaluations.selectSingleNode("//Evaluation[1]").selectSingleNode("ImageBase64String").innerText);
    },
    getImageFromString: function (base64String) {
        return "data:image/png;base64," + base64String;
    },

    getValuationData: function (group, evaluation) {
        var gestureName = evaluation.selectSingleNode("Name").innerText;
        return this.getGesture(gestureName, group, evaluation);
    },
    getGesture: function (gestureName, group, evaluation) {
        var gesture = GestureData[gestureName];
        return {
            title: gesture.title,
            subtitle: gesture.subtitle,
            description: gesture.description,
            content: gesture.content,
            group: group,
            backgroundImage: this.getImageFromString(evaluation.selectSingleNode("ImageBase64String").innerText)
        };
    },
    appendPageContent: function (evaluations) {
        evaluations.forEach(function (evaluation) {
            this.list.push(evaluation);
        }.bind(this));
    }
};

(function () {
    "use strict";
    Evaluation.init();
    Evaluation.loadEvaluations();
})();