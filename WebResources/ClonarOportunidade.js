if (typeof (Logistics) === "undefined") Logistics = {}
if (typeof (Logistics.Opportunity) === "undefined") Logistics.Opportunity = {} 


Logistics.Opportunity = {
    OnClick: function (executionContext) {
        var formContext = executionContext
        formContext.ui.setFormNotification("Clonando Oportunidade...", "INFO", "notification")
        
	      var id = formContext.data.entity.getId().replace("{", "").replace("}", "")

        var req = new XMLHttpRequest();
        req.open("POST", Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.2/opportunities(" + id + ")/Microsoft.Dynamics.CRM.lgs_ClonarOportunidades", true);
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("Accept", "application/json");
        req.onreadystatechange = function () {
        	if (this.readyState === 4) {
        		req.onreadystatechange = null;
        		if (this.status === 200 || this.status === 204) {
        			var result = JSON.parse(this.response);
        			console.log(result);
        			var newid = result["newid"];
              var pageInput = {
              pageType: "entityrecord",
              entityName: "opportunity",
              entityId: newid
              }
              Xrm.Navigation.navigateTo(pageInput)
        		} else {
        			console.log(this.responseText);
        		}
        	}
        };
        req.send();
        
    }
} 