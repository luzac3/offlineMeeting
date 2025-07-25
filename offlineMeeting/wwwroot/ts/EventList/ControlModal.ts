export class ControlModal {
    setControl = () => {
        $(".closeModal").on("click", () => {
            this.hidden();
        });
        $("#modalWrapper").on("click", (event) => {
            event.target.closest("#modalWindow") ?? this.hidden();
        });
        $("#newEvent").on("click", () => {
            this.show();
        });
    }

    private show = () => {
        $("#modalWrapper").removeClass("hidden");
    }

    private hidden = () => {
        $("#modalWrapper").addClass("hidden");
    }
}