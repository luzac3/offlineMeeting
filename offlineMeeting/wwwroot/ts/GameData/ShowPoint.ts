import { FetchApi } from "@root/share/FetchApi";
import { SetEventListner } from "@root/share/SetEventListner";

export class ShowPoint {
    url: string;
    method: string;
    headers: { [key: string]: string };
    responseKind: string;
    showPointProperty: {
        [key: string]: string |
        string[] |
        { [key: string]: string } |
        null
    } = {
        EventNumber: null,
        GameNumber: null
    };

    constructor() {
        this.url = '/GameData/ShowPoint';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";

        this.showPointProperty.EventNumber
            = (<HTMLInputElement>document.getElementById("eventNumber")).value;

        this.showPointProperty.GameNumber
            = (<HTMLInputElement>document.getElementById("gameNumber")).value;
    }

    setStartEvent = () => {
        SetEventListner.setEvent(
            document.getElementById("listModalWindow"),
            "click",
            "#showPointButton",
            () => {
                this.show().then((data: string) => {
                    document.getElementById("showPointModalWindow")!.innerHTML = data;
                    document.getElementById("showPointModalWrapper")!.classList.remove("hidden");
                });
            }
        );
    }

    private show = async (): Promise<string> => {
        const fetchApi = new FetchApi();

        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            this.showPointProperty,
            this.responseKind
        ).then(async (data: string) => {
            return data;
        }).catch(e => {
            throw e;
        });
    }
}