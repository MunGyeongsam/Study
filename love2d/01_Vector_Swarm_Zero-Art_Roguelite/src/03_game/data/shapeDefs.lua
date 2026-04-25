-- shapeDefs.lua
-- Auto-generated from curve curation summary.
-- Unclassified curves are intentionally excluded.

local M = {}

M.targetRadius = 1.0

M.groups = {
    enemy = {
        "Rose 3",
        "Rose 5",
        "Rose 7/3",
        "Folium (3-leaf)",
        "Astroid",
        "Deltoid",
        "Epicycloid (k=3)",
        "Epicycloid (k=4)",
        "Epicycloid (k=5)",
        "Epicycloid (k=6)",
        "Nephroid",
        "Heart Curve",
        "Cornoid",
        "Hypocycloid (k=5)",
        "Lissajous (3:2)",
        "Booth's Lemniscate",
        "Hypotrochoid (R=7,r=3,d=4)",
        "Epitrochoid (R=5,r=2,d=2)",
        "Spirograph",
        "Reuleaux Triangle",
        "Bifolium",
        "Quadrifolium",
        "Ranunculoid (k=5)",
        "Hypotrochoid (R=5,r=3,d=5)",
        "Epitrochoid (R=3,r=1,d=1)",
        "Rose 5/4",
        "Rose 8/3",
        "Wavy Circle (19:3)",
        "Lissajous (5:4)",
        "Hypotrochoid (R=7,r=3,d=2)",
    },
    boss = {
        "Butterfly (Fay)",
        "Gerono Lemniscate",
        "Maurer Rose (n=6, d=71)",
        "Rose 5/4",
        "Rose 8/3",
        "Wavy Circle (19:3)",
        "Lissajous (5:4)",
        "Hypotrochoid (R=7,r=3,d=2)",
    },
    overlay = {
        "Lemniscate",
        "Vesica Piscis",
        "Bicorn",
        "Devil's Curve",
        "Koch Edge (iter 2)",
    },
    bullet = {
        "Cardioid",
        "Limacon (inner loop)",
        "Logarithmic Spiral",
        "Lituus",
        "Cissoid",
        "Cayley's Sextic",
        "Freeth's Nephroid",
        "Ophiuride",
        "Strophoid",
        "Fermat Spiral",
    },
    excluded = {
        "Superellipse (n=4)",
        "Kampyle of Eudoxus",
        "Cassini Oval",
        "Conchoid of Nicomedes",
    },
}

M.usable = {}
for i = 1, #M.groups.enemy do M.usable[M.groups.enemy[i]] = true end
for i = 1, #M.groups.boss do M.usable[M.groups.boss[i]] = true end
for i = 1, #M.groups.overlay do M.usable[M.groups.overlay[i]] = true end
for i = 1, #M.groups.bullet do M.usable[M.groups.bullet[i]] = true end

M.normalized = {
    ["Astroid"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -1.0000, xMax = 1.0000, yMin = -1.0000, yMax = 1.0000 },
        normalizedBounds = { xMin = -1.0000, xMax = 1.0000, yMin = -1.0000, yMax = 1.0000 },
    },
    ["Bifolium"] = {
        usage = "enemy",
        centerOffset = { x = 0.0000, y = 0.1250 },
        maxRadius = 0.3322,
        scaleToUnitRadius = 3.0101,
        bounds = { xMin = -0.3246, xMax = 0.3246, yMin = 0.0000, yMax = 0.2500 },
        normalizedBounds = { xMin = -0.9771, xMax = 0.9771, yMin = -0.3763, yMax = 0.3763 },
    },
    ["Booth's Lemniscate"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -1.0000, xMax = 1.0000, yMin = -0.3535, yMax = 0.3535 },
        normalizedBounds = { xMin = -1.0000, xMax = 1.0000, yMin = -0.3535, yMax = 0.3535 },
    },
    ["Butterfly (Fay)"] = {
        usage = "boss",
        centerOffset = { x = 0.5651, y = -0.0000 },
        maxRadius = 4.6515,
        scaleToUnitRadius = 0.2150,
        bounds = { xMin = -2.4814, xMax = 3.9234, yMin = -3.6304, yMax = 3.6304 },
        normalizedBounds = { xMin = -0.6550, xMax = 0.7220, yMin = -0.7805, yMax = 0.7805 },
    },
    ["Cornoid"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = 0.0000 },
        maxRadius = 1.4141,
        scaleToUnitRadius = 0.7072,
        bounds = { xMin = -1.0000, xMax = 1.0000, yMin = -1.4140, yMax = 1.4140 },
        normalizedBounds = { xMin = -0.7072, xMax = 0.7072, yMin = -0.9999, yMax = 0.9999 },
    },
    ["Deltoid"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.4998, xMax = 1.0000, yMin = -0.8656, yMax = 0.8656 },
        normalizedBounds = { xMin = -0.4998, xMax = 1.0000, yMin = -0.8656, yMax = 0.8656 },
    },
    ["Epicycloid (k=3)"] = {
        usage = "enemy",
        centerOffset = { x = 0.0000, y = -0.0000 },
        maxRadius = 1.6667,
        scaleToUnitRadius = 0.6000,
        bounds = { xMin = -1.6667, xMax = 1.3484, yMin = -1.5851, yMax = 1.5851 },
        normalizedBounds = { xMin = -1.0000, xMax = 0.8090, yMin = -0.9511, yMax = 0.9511 },
    },
    ["Epicycloid (k=4)"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.4995,
        scaleToUnitRadius = 0.6669,
        bounds = { xMin = -1.2990, xMax = 1.2990, yMin = -1.2990, yMax = 1.2990 },
        normalizedBounds = { xMin = -0.8663, xMax = 0.8663, yMin = -0.8663, yMax = 0.8663 },
    },
    ["Epicycloid (k=5)"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.4000,
        scaleToUnitRadius = 0.7143,
        bounds = { xMin = -1.4000, xMax = 1.2611, yMin = -1.3648, yMax = 1.3648 },
        normalizedBounds = { xMin = -1.0000, xMax = 0.9008, yMin = -0.9749, yMax = 0.9749 },
    },
    ["Epicycloid (k=6)"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = 0.0000 },
        maxRadius = 1.3333,
        scaleToUnitRadius = 0.7500,
        bounds = { xMin = -1.2317, xMax = 1.2317, yMin = -1.3333, yMax = 1.3333 },
        normalizedBounds = { xMin = -0.9238, xMax = 0.9238, yMin = -1.0000, yMax = 1.0000 },
    },
    ["Epitrochoid (R=3,r=1,d=1)"] = {
        usage = "enemy",
        centerOffset = { x = 0.0000, y = -0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -1.0000, xMax = 0.8090, yMin = -0.9511, yMax = 0.9511 },
        normalizedBounds = { xMin = -1.0000, xMax = 0.8090, yMin = -0.9511, yMax = 0.9511 },
    },
    ["Epitrochoid (R=5,r=2,d=2)"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = 0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.9397, xMax = 1.0000, yMin = -0.9843, yMax = 0.9843 },
        normalizedBounds = { xMin = -0.9397, xMax = 1.0000, yMin = -0.9843, yMax = 0.9843 },
    },
    ["Folium (3-leaf)"] = {
        usage = "enemy",
        centerOffset = { x = 0.0000, y = 0.2500 },
        maxRadius = 0.6644,
        scaleToUnitRadius = 1.5050,
        bounds = { xMin = -0.6492, xMax = 0.6492, yMin = 0.0000, yMax = 0.5000 },
        normalizedBounds = { xMin = -0.9771, xMax = 0.9771, yMin = -0.3763, yMax = 0.3763 },
    },
    ["Gerono Lemniscate"] = {
        usage = "boss",
        centerOffset = { x = -0.0000, y = 0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -1.0000, xMax = 1.0000, yMin = -0.4998, yMax = 0.4998 },
        normalizedBounds = { xMin = -1.0000, xMax = 1.0000, yMin = -0.4998, yMax = 0.4998 },
    },
    ["Heart Curve"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = 0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.9412, xMax = 0.9412, yMin = -1.0000, yMax = 0.7003 },
        normalizedBounds = { xMin = -0.9412, xMax = 0.9412, yMin = -1.0000, yMax = 0.7003 },
    },
    ["Hypocycloid (k=5)"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.8090, xMax = 1.0000, yMin = -0.9511, yMax = 0.9511 },
        normalizedBounds = { xMin = -0.8090, xMax = 1.0000, yMin = -0.9511, yMax = 0.9511 },
    },
    ["Hypotrochoid (R=5,r=3,d=5)"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = 0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.8219, xMax = 1.0000, yMin = -0.9539, yMax = 0.9539 },
        normalizedBounds = { xMin = -0.8219, xMax = 1.0000, yMin = -0.9539, yMax = 0.9539 },
    },
    ["Hypotrochoid (R=7,r=3,d=2)"] = {
        usage = "enemy,boss",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.9050, xMax = 1.0000, yMin = -0.9755, yMax = 0.9755 },
        normalizedBounds = { xMin = -0.9050, xMax = 1.0000, yMin = -0.9755, yMax = 0.9755 },
    },
    ["Hypotrochoid (R=7,r=3,d=4)"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.9029, xMax = 1.0000, yMin = -0.9754, yMax = 0.9754 },
        normalizedBounds = { xMin = -0.9029, xMax = 1.0000, yMin = -0.9754, yMax = 0.9754 },
    },
    ["Lissajous (3:2)"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = 0.0000 },
        maxRadius = 1.3479,
        scaleToUnitRadius = 0.7419,
        bounds = { xMin = -1.0000, xMax = 1.0000, yMin = -1.0000, yMax = 1.0000 },
        normalizedBounds = { xMin = -0.7419, xMax = 0.7419, yMin = -0.7419, yMax = 0.7419 },
    },
    ["Lissajous (5:4)"] = {
        usage = "enemy,boss",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.3917,
        scaleToUnitRadius = 0.7185,
        bounds = { xMin = -1.0000, xMax = 1.0000, yMin = -0.9998, yMax = 0.9998 },
        normalizedBounds = { xMin = -0.7185, xMax = 0.7185, yMin = -0.7184, yMax = 0.7184 },
    },
    ["Maurer Rose (n=6, d=71)"] = {
        usage = "boss",
        centerOffset = { x = 0.0000, y = 0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.9659, xMax = 0.9659, yMin = -0.9659, yMax = 0.9659 },
        normalizedBounds = { xMin = -0.9659, xMax = 0.9659, yMin = -0.9659, yMax = 0.9659 },
    },
    ["Nephroid"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.7061, xMax = 0.7061, yMin = -1.0000, yMax = 1.0000 },
        normalizedBounds = { xMin = -0.7061, xMax = 0.7061, yMin = -1.0000, yMax = 1.0000 },
    },
    ["Quadrifolium"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = 0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -1.0000, xMax = 1.0000, yMin = -1.0000, yMax = 1.0000 },
        normalizedBounds = { xMin = -1.0000, xMax = 1.0000, yMin = -1.0000, yMax = 1.0000 },
    },
    ["Ranunculoid (k=5)"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -1.0000, xMax = 0.9008, yMin = -0.9749, yMax = 0.9749 },
        normalizedBounds = { xMin = -1.0000, xMax = 0.9008, yMin = -0.9749, yMax = 0.9749 },
    },
    ["Reuleaux Triangle"] = {
        usage = "enemy",
        centerOffset = { x = 0.0000, y = 0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.8660, xMax = 0.8660, yMin = -1.0000, yMax = 0.7321 },
        normalizedBounds = { xMin = -0.8660, xMax = 0.8660, yMin = -1.0000, yMax = 0.7321 },
    },
    ["Rose 3"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.5625, xMax = 1.0000, yMin = -0.8800, yMax = 0.8800 },
        normalizedBounds = { xMin = -0.5625, xMax = 1.0000, yMin = -0.8800, yMax = 0.8800 },
    },
    ["Rose 5"] = {
        usage = "enemy",
        centerOffset = { x = -0.0000, y = 0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.8169, xMax = 1.0000, yMin = -0.9528, yMax = 0.9528 },
        normalizedBounds = { xMin = -0.8169, xMax = 1.0000, yMin = -0.9528, yMax = 0.9528 },
    },
    ["Rose 5/4"] = {
        usage = "enemy,boss",
        centerOffset = { x = -0.0000, y = 0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -1.0000, xMax = 1.0000, yMin = -0.9682, yMax = 0.9682 },
        normalizedBounds = { xMin = -1.0000, xMax = 1.0000, yMin = -0.9682, yMax = 0.9682 },
    },
    ["Rose 7/3"] = {
        usage = "enemy",
        centerOffset = { x = 0.0000, y = -0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.9160, xMax = 1.0000, yMin = -0.9781, yMax = 0.9781 },
        normalizedBounds = { xMin = -0.9160, xMax = 1.0000, yMin = -0.9781, yMax = 0.9781 },
    },
    ["Rose 8/3"] = {
        usage = "enemy,boss",
        centerOffset = { x = -0.0000, y = -0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -1.0000, xMax = 1.0000, yMin = -1.0000, yMax = 1.0000 },
        normalizedBounds = { xMin = -1.0000, xMax = 1.0000, yMin = -1.0000, yMax = 1.0000 },
    },
    ["Spirograph"] = {
        usage = "enemy",
        centerOffset = { x = 0.0012, y = -0.0254 },
        maxRadius = 1.2239,
        scaleToUnitRadius = 0.8171,
        bounds = { xMin = -0.9803, xMax = 1.2000, yMin = -1.1434, yMax = 1.1434 },
        normalizedBounds = { xMin = -0.8020, xMax = 0.9795, yMin = -0.9135, yMax = 0.9551 },
    },
    ["Wavy Circle (19:3)"] = {
        usage = "enemy,boss",
        centerOffset = { x = -0.0000, y = 0.0000 },
        maxRadius = 1.0000,
        scaleToUnitRadius = 1.0000,
        bounds = { xMin = -0.9972, xMax = 0.9972, yMin = -0.9890, yMax = 1.0000 },
        normalizedBounds = { xMin = -0.9972, xMax = 0.9972, yMin = -0.9890, yMax = 1.0000 },
    },
}

-- Reverse lookup: curveName → tag set {enemy=true, boss=true, ...}
M._tagLookup = {}
for groupName, list in pairs(M.groups) do
    for i = 1, #list do
        local name = list[i]
        if not M._tagLookup[name] then
            M._tagLookup[name] = {}
        end
        M._tagLookup[name][groupName] = true
    end
end

function M.isUsable(curveName)
    return M.usable[curveName] == true
end

--- Returns tag set table (e.g. {enemy=true, boss=true}) or empty table.
function M.getTags(curveName)
    return M._tagLookup[curveName] or {}
end

--- Backward compat: returns first tag name or "none".
function M.getGroup(curveName)
    local tags = M._tagLookup[curveName]
    if not tags then return "none" end
    for k, _ in pairs(tags) do return k end
    return "none"
end

function M.getNormalization(curveName)
    return M.normalized[curveName]
end

return M

