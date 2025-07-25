import { FetchApi } from "@root/share/FetchApi";
import { SetEventListner } from "@root/share/SetEventListner";

export class StartGame {
    url: string;
    method: string;
    headers: { [key: string]: string };
    responseKind: string;
    startGameProperty: {
        [key: string]: string |
        string[] |
        { [key: string]: string } |
        null
    } = {
        EventNumber: null,
        HandNumber: null,
        HandSubNumber: null,
        UsersList: null,
        DirectionDictionary: null,
        VideoDictionary: null,
        VideoNumberDictionary: null
    };

    constructor() {
        this.url = '/Event/StartGame';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "json";

        this.startGameProperty.EventNumber
            = (<HTMLInputElement>document.getElementById("eventNumber")).value;
    }

    setStartEvent = () => {
        SetEventListner.setEvent(
            document.getElementById("listModalWindow"),
            "click",
            "#listStartGameButton",
            () => {
                this.start().then((data: { [key: string]: string }) => {
                    this.setGameData(data);
                });
            }
        );
    }

    private setGameData = (data: {[key:string]: string}) => {
        console.log(data);
        const gameData = data["gameData"];
        const videoController = data["videoController"];
        const gameDataElement = <HTMLElement>document.getElementById("mainContents")
        const videoControllElement = <HTMLElement>document.getElementById("listModalWindow");

        gameDataElement.innerHTML = gameData;
        videoControllElement.innerHTML = videoController;
    }

    private start = async (): Promise<{ [key: string]: string }> => {
        let userList: string[] = [];
        let directionDic: { [key: string]: string } = {};
        let videoDic: { [key: string]: string } = {};
        let videoNumberDic: { [key: string]: string } = {};
        const userListElement = document.getElementsByClassName("userList");
        if (userListElement.length) {
            Array.from(userListElement).forEach((userElement, index) => {
                const userCd = (<HTMLSelectElement>userElement).value;
                const directionElement = (<HTMLSelectElement>userElement)
                    .closest(".videoController")!
                    .querySelector(".directionList")
                    ;
                const VideoElement = (<HTMLSelectElement>userElement)
                    .closest(".videoController")!
                    .querySelector(".cameraSelector")
                    ;
                if (userCd == "99") {
                    // 天吊のコードを除外
                    return;
                }
                if (userList.length >= 4) {
                    return;
                }
                userList.push(userCd);
                directionDic[userCd] = (<HTMLSelectElement>directionElement).value;
                videoDic[userCd] = (<HTMLSelectElement>VideoElement).value;
                videoNumberDic[userCd] = index.toString();
            });

            const checkUserCount = Array.from(new Set(userList));
            if (checkUserCount.length < 4) {
                alert("ユーザーが重複しています");
                throw new Error("user Duplicated");
            }
        }
        this.startGameProperty.UsersList = userList;
        this.startGameProperty.DirectionDictionary = directionDic;
        this.startGameProperty.VideoDictionary = videoDic;
        this.startGameProperty.VideoNumberDictionary = videoNumberDic;

        this.startGameProperty.HandNumber
            = (<HTMLInputElement>document.getElementById("handNumber")).value;

        this.startGameProperty.HandSubNumber
            = (<HTMLInputElement>document.getElementById("handSubNumber")).value;

        const startGameProperty = new StartGameProperty(
            parseInt((<HTMLInputElement>document.getElementById("eventNumber")).value),
            userList,
            directionDic
        );

        const fetchApi = new FetchApi();

        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            this.startGameProperty,
            this.responseKind
        ).then(async (data: { [key: string]: string}) => {
            return data;
        }).catch(e => {
            throw e;
        });
    }
}

class StartGameProperty {
    EventNumber: number;
    UsersList: string[] | null;
    DirectionDictionary: { [key: string]: string } | null;

    constructor(
        EventNumber: number,
        UsersList: string[] | null,
        DirectionDictionary: { [key: string]: string } | null
    ) {
        this.EventNumber = EventNumber;
        this.UsersList = UsersList;
        this.DirectionDictionary = DirectionDictionary;
    }
}