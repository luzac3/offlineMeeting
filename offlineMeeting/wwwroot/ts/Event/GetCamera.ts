import { SetEventListner } from "@root/share/SetEventListner"
import { FetchApi } from "@root/share/FetchApi";
import { GetCameraInformation } from "./GetCameraInformation"

export class GetCamera {
    url: string;
    method: string;
    headers: { [key: string]: string };
    responseKind: string;
    getCameraProperty: {
        [key: string]: string |
        string[] |
        { [key: string]: string } |
        { [key: string]: string }[] |
        null
    } = {
        EventNumber: null,
        GameNumber: null,
        CameraContainer: null
    };
    cameraContainer: { [key: string]: string }[] = [];

    constructor() {
        this.url = '/Event/GetCamera';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";

        this.getCameraProperty.EventNumber
            = (<HTMLInputElement>document.getElementById("eventNumber")).value;

        this.getCameraProperty.GameNumber
            = (<HTMLInputElement>document.getElementById("gameNumber")).value;
    }

    static cameraContainer: { [key: string]: string }[] = [];

    setGetCamerEvent = () => {
        SetEventListner.setEvent(
            document.getElementById("listModalWindow"),
            "click",
            "#getCameraButton",
            async () => {
                this.getCameraProperty.CameraContainer = await GetCameraInformation.setDevices();
                const videoHtml = await this.get();

                document.getElementById("listModalWindow")!.innerHTML = videoHtml;
            }
        );
    }

    private get = async () => {
        const fetchApi = new FetchApi();

        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            this.getCameraProperty,
            this.responseKind
        ).then(async (data: string ) => {
            return data;
        }).catch(e => {
            throw e;
        });
    }
}