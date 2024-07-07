var OTSServer;
(function (OTSServer) {
    var wwwroot;
    (function (wwwroot) {
        class DynamicTable {
        }
        function sayHello() {
            return "Hello";
        }
        wwwroot.sayHello = sayHello;
    })(wwwroot = OTSServer.wwwroot || (OTSServer.wwwroot = {}));
})(OTSServer || (OTSServer = {}));
//# sourceMappingURL=DynamicTable.js.map