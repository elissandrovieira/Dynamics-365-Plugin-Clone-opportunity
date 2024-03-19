if (typeof (Logistics) === "undefined") Logistics = {}
if (typeof (Logistics.Util) === "undefined") Logistics.Util = {}

Logistics.Util = {

    Alert: function (title, description) {

        const textSettings = {
            confirmButtonLabel: "OK",
            title: title,
            text: description
        }

        const optionsSettings = {
            height: 120,
            width: 200
        }

        Xrm.Navigation.openAlertDialog(textSettings, optionsSettings)
    }
}