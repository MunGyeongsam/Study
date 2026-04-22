-- Stage Story Data — "Memory Odyssey"
-- 손상된 메모리 시스템을 순회하는 프로그램의 여정.
-- 각 스테이지가 수학/CS 개념 하나.
--
-- 수정 가이드:
-- 1. NORMAL_STORIES: 일반 스테이지 클리어 시 글리치 텍스트 (2~3초)
-- 2. BOSS_STORIES: 보스 클리어 시 팝업 씬 (별도 오버레이)
-- 3. ENDLESS_QUOTES: Endless(16+) 랜덤 순환
-- 4. 새 스테이지 추가 시 해당 테이블에 항목만 추가하면 됨

local M = {}

-- ─── Normal Stage Clear (글리치 텍스트) ─────────────────────────
-- key = stage number (보스 스테이지 제외)
M.NORMAL = {
    [1]  = { concept = "Binary",     text = "01100010 -- The world speaks in two." },
    [2]  = { concept = "Logic Gate", text = "AND, OR, NOT -- truth has only three gates." },
    [4]  = { concept = "Variable",   text = "x := chaos -- a name given to the unnamed." },
    [5]  = { concept = "Loop",       text = "while(alive) { fight(); } -- same path, never the same." },
    [7]  = { concept = "Array",      text = "arr[0]: You are here. Position is identity." },
    [8]  = { concept = "Sort",       text = "O(n log n) -- the price of order in entropy." },
    [10] = { concept = "Tree",       text = "if(left || right) -- every decision branches." },
    [11] = { concept = "Graph",      text = "V={you}, E={connections} -- isolation is a node with no edges." },
    [13] = { concept = "Infinity",   text = "lim(n->inf) -- not a place, but a direction." },
    [14] = { concept = "Chaos",      text = "dx0=0.001 -- deterministic, yet unpredictable." },
}

-- ─── Boss Stage Clear (팝업 씬) ─────────────────────────────────
-- key = boss type name
M.BOSS = {
    NULL = {
        title = "DEREFERENCED",
        body  = "*ptr = NULL;\nYou followed a pointer to nothing.\nThe absence that crashes worlds\nnow lies resolved.",
    },
    STACK = {
        title = "UNWOUND",
        body  = "pop(); pop(); pop();\nLast in, first out.\nThe deeper the call,\nthe harder the return.\nYou climbed back.",
    },
    HEAP = {
        title = "FREED",
        body  = "free(max);\nPriority determined survival.\nThe maximum always rose to the top.\nUntil you arrived.",
    },
    RECURSION = {
        title = "BASE CASE",
        body  = "return 1;\nTo understand recursion,\nfirst understand recursion.\nYou found the base case.",
    },
    OVERFLOW = {
        title = "HANDLED",
        body  = "catch(overflow) {}\nWhen the count exceeds its container,\neverything breaks.\nYou contained the uncontainable.",
    },
}

-- ─── Endless Quotes (16+, 랜덤 순환) ───────────────────────────
M.ENDLESS = {
    "e = 2.71828... -- growth never stops.",
    "P != NP -- some answers can't be shortcut.",
    "pi = 3.14159... -- the circle never closes.",
    "0! = 1 -- even nothing counts for something.",
    "i^2 = -1 -- imagination made real.",
    "Godel: some truths can never be proven.",
    "Turing: there are things no machine can decide.",
    "Fibonacci: 1, 1, 2, 3, 5, 8... nature's hidden code.",
    "Shannon: information is the resolution of uncertainty.",
    "Euler: e^(i*pi) + 1 = 0 -- the most beautiful equation.",
}

return M
