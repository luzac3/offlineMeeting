import { FetchApi } from "@root/share/FetchApi";
import { SetEventListner } from "@root/share/SetEventListner";

export class RegisterEndGame {
    url: string;
    method: string;
    headers: { [key: string]: string };
    responseKind: string;
    endGameProperty: {
        [key: string]: string | null
    } = {
        EventNumber: null,
        GameNumber: null
        };

    setGameEndEvent = () => {
        SetEventListner.setEvent(
            document.getElementById("listModalWindow") as HTMLElement,
            "click",
            "#endGamebutton",
            () => {
                this.setGameEnd().then(() => {
                    // 実際はリザルトの表示を入れたほうが良さそう
                    window.location.reload();
                });
            }
        )
    }

    constructor() {
        this.url = '/Event/EndGame';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "json";

        this.endGameProperty.EventNumber
            = (<HTMLInputElement>document.getElementById("eventNumber")).value;

        this.endGameProperty.GameNumber
            = (<HTMLInputElement>document.getElementById("gameNumber")).value;
    }

    private setGameEnd = async (): Promise<void> => {
        const fetchApi = new FetchApi();

        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            this.endGameProperty,
            this.responseKind
        ).then(async (data: boolean) => {
            if (data) {
                window.location.reload();
            }
        }).catch(e => {
            throw e;
        });
    }
}