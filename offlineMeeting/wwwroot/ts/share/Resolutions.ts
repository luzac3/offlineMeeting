export class Resolutions {
    ResolutionArray: { [key: string]: { [key: string]: number } } = {};

    constructor() {
        this.ResolutionArray = {
            WIDE: { width: 320, height: 180 },
            QVGA: { width: 320, height: 240 },
            WQVGA: { width: 480, height: 272 },
            VGA: { width: 640, height: 480 },
            SDTV: { width: 720, height: 480 },
            SVGA: { width: 800, height: 600 },
            XGA: { width: 1024, height: 768 },
            HDTV: { width: 1280, height: 720 },
        }
    }

    searchNearResolution = (width: number, height: number) => {
        // widthかHeightの一番近いところを探し、その解像度でCanvasを生成
        // 一番誤差が小さい画像を探す
        let nearWidth = 0;
        let nearHeight = 0;
        let heightResolution: { [key: string]: number } = {};
        let widthResolution: { [key: string]: number } = {};

        // widthチェック
        Object.keys(this.ResolutionArray).forEach((key) => {
            if (this.ResolutionArray[key].w > width) {
                return;
            }
            nearWidth = this.ResolutionArray[key].w;
            widthResolution = this.ResolutionArray[key];
        });

        // heightチェック
        Object.keys(this.ResolutionArray).forEach((key) => {
            if (this.ResolutionArray[key].h > height) {
                return;
            }
            nearHeight = this.ResolutionArray[key].h;
            heightResolution = this.ResolutionArray[key];
        });

        // heightとwidthのどちらが大きいか
        if (nearHeight > nearWidth) {
            return widthResolution;
        } else {
            return heightResolution;
        }
    }
}