window.bootstrap_tooltip_timers = window.bootstrap_tooltip_timers || {};

export function setup(id, options) {
    try {
        destroy(id);
        var identifier = "bootstrap-tooltip-" + id;
        var element = document.getElementById(identifier);

        var oOptions = JSON.parse(options);
        var tooltip = bootstrap.Tooltip.getOrCreateInstance(element, oOptions);
    } catch (e) {
        console.error("Error from Tooltip Setup " + e.message);
        console.error("Parameters = ID: " + id + " | Options: " + options);
        console.error(e);
    }
}

export function destroy(id) {
    try {
        var identifier = "bootstrap-tooltip-" + id;
        var element = document.getElementById(identifier);

        var tooltip = bootstrap.Tooltip.getInstance(element);

        if (tooltip != null && typeof tooltip !== 'undefined') {
            tooltip.dispose();
        }
    } catch (e) {
        console.error("Error from Tooltip Destroy  " + e.message);
        console.error("Parameters = ID: " + id);
        console.error(e);
    }
}

export function update(id, options) {
    try {
        setup(id, options);
    } catch (e) {
        console.error("Error from Tooltip Update " + e.message);
        console.error("Parameters = ID: " + id + " | Options: " + options);
        console.error(e);
    }
}
