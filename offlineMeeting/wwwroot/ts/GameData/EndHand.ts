import { SignalR } from "@root/share/SignalR";
import { RegisterEndHand } from "./RegisterEndHand";
import { SetEventListner } from "@root/share/SetEventListner";

export class EndHand {
    setEndHandRegisterEvent = (signalR: SignalR) => {
        SetEventListner.setEvent(
            document.getElementById("endHandModalWindow") as HTMLElement,
            "click",
            "#correctModal",
            () => {
                const registerEndHand = new RegisterEndHand();
                registerEndHand.register();
            }
        )

        signalR.get("SendEndHandJson", (endHandJson) => {
            endHandJson  = <{ [key: string]: string }>endHandJson;
            const resultModal = document.getElementById("resultModalWindow") as HTMLElement;
            resultModal.innerHTML = endHandJson["result"] as string;

            document.getElementById("resultModalWrapper")?.classList.remove("hidden");

            document.getElementById("nextHand")?.addEventListener("click", () => {
                document.getElementById("nowHand")?.classList.remove("hidden");
                document.getElementById("nextResult")?.classList.remove("hidden");
                document.getElementById("nowResult")?.classList.add("hidden");
            });

            document.getElementById("nowHand")?.addEventListener("click", () => {
                document.getElementById("nowHand")?.classList.add("hidden");
                document.getElementById("nextResult")?.classList.add("hidden");
                document.getElementById("nowResult")?.classList.remove("hidden");
            });

            document.getElementById("resultConfirm")?.addEventListener("click", () => {
                if (document.getElementById("mainContents") != null) {
                    document.getElementById("mainContents")!.innerHTML = endHandJson["gameData"] as string;
                }

                document.getElementById("resultModalWrapper")?.classList.add("hidden");
                document.getElementById("endHandModalWrapper")?.classList.add("hidden");
                if (document.getElementById("listStartGameButton") != null) {
                    // ボタンイベントをキック
                    document.getElementById("listStartGameButton")!.click();
                }
            });
        });
    }
}