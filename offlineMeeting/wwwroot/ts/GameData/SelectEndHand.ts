import { FetchApi } from "@root/share/FetchApi";

export class SelectEndHand {
    url: string;
    method: string;
    headers: { [key: string]: string };
    responseKind: string;
    eventNumber: string;

    constructor() {
        this.url = '/GameData/EndHandChangeDisplay';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";

        this.eventNumber = (<HTMLInputElement>document.getElementById("eventNumber")).value;
    }

    setHoraKindEvent = () => {
        document.getElementById("horaKind")?.addEventListener(
            "change",
            () => {
                this.sendEndHand();
            }
        );
    }

    private sendEndHand = async () => {
        const horaKind = $("#horaKind").val()?.toString() ?? "";
        this.rewritePartial({
            EventNumber: this.eventNumber,
            HoraKind: horaKind
        });
    }

    private changeToUserEventListner = (horaKind: string) => {
        const toUserElementList = document.getElementsByClassName("toUser");
        toUserElementList[0].addEventListener("change", () => {
            this.changeToUserEvent(horaKind, <HTMLSelectElement>toUserElementList[0])
        });
    }

    private changeToUserEvent = (horaKind: string, toUserElement: HTMLSelectElement) => {
        const toUser = toUserElement.value.toString() ?? "";
        const index = toUserElement.selectedIndex;
        const parent = $("#parent").val()?.toString() ?? "";
        const isPao = (document.getElementById("pao") as HTMLInputElement).checked;
        return this.rewritePartial(
            {
                EventNumber: this.eventNumber,
                HoraKind: horaKind,
                Parent: parent,
                ToUser: toUser,
                IsPao: isPao
            }
        ).then(() => {
            console.log("leach");
            const newToUserElement = document.getElementsByClassName("toUser")[0] as HTMLSelectElement;
            newToUserElement.options[index].selected = true;
            if (parent == toUser) {

            }
        });
    }

    private rewritePartial = async (
        body: { [key: string]: string | boolean }
    ) => {
        let fetchApi = new FetchApi();
        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            body,
            this.responseKind
        ).then((data: string) => {
            $("#endGame").empty();
            $("#endGame").append(data);
            const horaKind = $("#horaKind").val()?.toString() ?? "";
            if (horaKind == "1") {
                this.changeToUserEventListner(horaKind);
            }
            return "";
        }).catch(e => {
            console.log(e);
            //fixme エラー処理
        });
    }
}