import { FetchApi } from "@root/share/FetchApi";

export class StartEvent {
    url: string;
    method: string;
    headers: { [key: string]: string };
    responseKind: string;
    constructor() {
        this.url = '/Event/Index';
        this.method = 'GET';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }
    private startEvent = async (formDataEntryValue: { [key: string]: string }) => {
        const fetchApi = new FetchApi();

        let url = this.url;
        url += "?";
        Object.keys(formDataEntryValue).forEach(key => {
            url += key + "=" + formDataEntryValue[key] + "&";
        });
        url = url.slice(0, -1);
        fetchApi.send(
            url,
            this.method,
            this.headers,
            null,
            this.responseKind
        ).then(() => {
            location.href = url;
        }).catch(e => {
            console.log(e);
        });
    }

    start = () => {
        $(".start").on("click", (event) => {
            const eventNumber = event.target.parentElement?.getAttribute("data-eventNumber");
            if (!eventNumber) {
                console.log("event Number error");
                return;
            }
            this.startEvent({
                EventNumber: eventNumber
            });
        });
    }
}