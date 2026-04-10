
--copy this file to 'C:\Program Files (x86)\Lua\5.1\lua'
--and require 'kutil'

local io = require'io'

local kutil = {}
--@{ util
-------------------------------------------------------------------------------
local tbl_printed = {}

-------------------------------------------------------------------------------
local function print_table_rcv(var, depth)
    if tbl_printed[var] then
        return
    end
    tbl_printed[var] = true
    local str = string.rep('-', depth*4)
    for k,v in pairs(var) do
        print(str, k, v)
        if type(v) == 'table' then
            print_table_rcv(v, depth + 1)
        end
    end
end

-------------------------------------------------------------------------------
-- print_table: 테이블을 재귀적으로 보기 좋게 출력
-- 사용예: kutil.print_table({a=1, b={2,3}})
function kutil.print_table(var)
    print(var)
    if type(var) == 'table' then
        tbl_printed = {}
        print_table_rcv(var, 1)
    end
end
--@}


-- printf: C 스타일 포맷 문자열로 출력
-- 사용예: kutil.printf("%d %s", 1, "hi")
function kutil.printf(fmt, ...)
    print(string.format(fmt, ...))
end

-- print_array: 배열을 인덱스와 함께 한 줄씩 출력
-- 사용예: kutil.print_array({"a", "b", "c"})
function kutil.print_array(arr)
    for i,v in ipairs(arr) do
        kutil.printf('[%d] : "%s"', i,v)
    end
end

-------------------------------------------------------------------------------
-- line iterator

--[[ example
local k = require'kutil'
str = require'str'
for n,l in k.lines(str) do
	print(n, l)
end
--]]
-------------------------------------------------------------------------------
-- lines: 문자열을 줄 단위로 순회하는 반복자 반환
-- 사용예:
-- for n, l in kutil.lines("a\nb\nc") do print(n, l) end
function kutil.lines (str)
	local i = 1
	local s = 1
	local e = string.find(str, '\n')

	return
	function ()

		if not s then
			return nil
		elseif not e then
			local from,to = s, -1
			s = nil
			return i, string.sub(str,from,to)
		end


		local n = i
		local from,to = s, e-1
		s = e+1
		e = string.find(str, '\n', s)
		i = i+1
		return n, string.sub(str,from,to)
	end
end


-- trim: 문자열 양쪽 공백 제거
-- 사용예: kutil.trim("  hello world  ") -- "hello world"
function kutil.trim(s)
  -- from PiL2 20.4
  return (s:gsub("^%s*(.-)%s*$", "%1"))
end

-- startwith: 문자열이 특정 문자열로 시작하는지 검사
-- 사용예: kutil.startwith("foobar", "foo") -- true
function kutil.startwith(s, w)
	return (string.find(s, w) == 1)
end

-- kutil.print_array(kutil.split('518 한글 - 테스트1', '%s+'))
-- split: 패턴 기준으로 문자열 분할
-- 사용예:
-- kutil.split("a,b,c", ",")           -- {"a","b","c"}
-- kutil.split("a b  c", "%s+")        -- {"a","b","c"}
-- kutil.split("a|b|c", "|")           -- {"a","b","c"}
-- kutil.split("a=b;c=d", "[;=]")      -- {"a","b","c","d"}
-- kutil.split("a,b,c")                 -- {"a","b","c"} (기본 공백 패턴)
function kutil.split(str, pat)
    pat = pat or '%s+'
    local t = {}  -- NOTE: use {n = 0} in Lua-5.0
    local fpat = "(.-)" .. pat
    local last_end = 1
    local s, e, cap = str:find(fpat, 1)
    while s do
        if s ~= 1 or cap ~= "" then
            table.insert(t,cap)
        end
        last_end = e+1
        s, e, cap = str:find(fpat, last_end)
    end
    if last_end <= #str then
        cap = str:sub(last_end)
        table.insert(t, cap)
    end
    return t
end

-- starts: 문자열이 특정 문자열로 시작하는지 검사 (startwith와 동일)
-- 사용예: kutil.starts("foobar", "foo") -- true
function kutil.starts(String,Start)
   return string.sub(String,1,string.len(Start))==Start
end

-- ends: 문자열이 특정 문자열로 끝나는지 검사
-- 사용예: kutil.ends("foobar", "bar") -- true
function kutil.ends(String,End)
   return End=='' or string.sub(String,-string.len(End))==End
end

-- repeats: 문자열 s를 n번 반복
-- 사용예: kutil.repeats("ab", 3) -- "ababab"
function kutil.repeats(s, n) return n > 0 and s .. kutil.repeats(s, n-1) or "" end

-- shallowcopy: 테이블 얕은 복사
-- 사용예: local t2 = kutil.shallowcopy(t1)
function kutil.shallowcopy(orig)
    local orig_type = type(orig)
    local copy
    if orig_type == 'table' then
        copy = {}
        for orig_key, orig_value in pairs(orig) do
            copy[orig_key] = orig_value
        end
    else -- number, string, boolean, etc
        copy = orig
    end
    return copy
end

-- arraycopy: 배열(순차 테이블) 얕은 복사
-- 사용예: local arr2 = kutil.arraycopy(arr1)
function kutil.arraycopy(orig)
	local copy = {}

	for i,v in ipairs(orig) do
		copy[i] = v
	end

	return copy
end


-- utf8charbytes: UTF-8 문자열에서 i번째 문자의 바이트 수 반환
-- 사용예: kutil.utf8charbytes("한", 1) -- 3
function kutil.utf8charbytes (s, i)
    -- argument defaults
    i = i or 1
    local c = string.byte(s, i)

    -- determine bytes needed for character, based on RFC 3629
    if c > 0 and c <= 127 then
        -- UTF8-1
        return 1
    elseif c >= 194 and c <= 223 then
        -- UTF8-2
        local c2 = string.byte(s, i + 1)
        return 2
    elseif c >= 224 and c <= 239 then
        -- UTF8-3
        local c2 = s:byte(i + 1)
        local c3 = s:byte(i + 2)
        return 3
    elseif c >= 240 and c <= 244 then
        -- UTF8-4
        local c2 = s:byte(i + 1)
        local c3 = s:byte(i + 2)
        local c4 = s:byte(i + 3)
        return 4
    end
end

-- returns the number of characters in a UTF-8 string
-- utf8len: UTF-8 문자열의 실제 문자 개수 반환
-- 사용예: kutil.utf8len("한글abc") -- 5
function kutil.utf8len (s)
    local pos = 1
    local bytes = string.len(s)
    local len = 0

    while pos <= bytes do
        local c = string.byte(s,pos)
        len = len + 1
        pos = pos + kutil.utf8charbytes(s, pos)
    end

    return len
end

-- save: 문자열을 파일로 저장 (overwrite=false면 이미 있으면 저장 안 함)
-- 사용예: kutil.save("out.txt", "hello")
function kutil.save(fullpath, str, overwrite)
    overwrite = overwrite or false

    if not overwrite then
        local f = io.open(fullpath, 'r')
        if f then
            f:close()
            print'error already exist'
            return
        end
    end

    local f,err = io.open(fullpath, 'wt')
    if f then
        f:write(str)
        f:close()
    else
        print('error', err)
    end
end


-- load: 파일 전체 내용을 문자열로 읽어 반환
-- 사용예: local s = kutil.load("out.txt")
function kutil.load(fullpath)
    local f = io.open(fullpath, 'r')
    if f then
        local content = f:read'*all'
        f:close()
        return content
    end
end




local function getKeys(tbl)
    local hash = {}
    for k in pairs(tbl) do
        if type(k) ~= 'number' or k < 1 or k > #tbl then
            hash[#hash + 1] = k
        end
    end

    table.sort(hash)
    return hash, #tbl
end
local function rvalueString(value)
    if string.find(value, '"') and string.find(value, "'")then
        return string.format("[==[%s]==]", value)
    elseif string.find(value, '"') then
        return string.format("'%s'", value)
    end

    return string.format('"%s"', value)
end

local function table2stringRecursive(lines, tbl, depth)
    local h,num = getKeys(tbl)
    local indent = string.rep('\t', depth)
    
    if depth == 1 and #h > 0 then
        lines[#lines + 1] = ''
    end

    for i,k in ipairs(h) do
        local value = tbl[k]
        local vType = type(value)
        local kType = type(k)

        local kStr
        if kType == 'string' then
            if string.find(k, '%s+') then
                kStr = string.format('["%s"]', tostring(k))
            else
                kStr = k
            end
        else
            kStr = string.format('[%s]', tostring(k))
        end

        local vStr = (vType == 'string') and rvalueString(value) or tostring(value)

        if kType == 'string' or kType == 'number' then
            if vType == 'table' then
                lines[#lines + 1] = string.format('%s%s = {', indent, kStr)
                table2stringRecursive(lines, value, depth + 1)
                lines[#lines + 1] = string.format('%s},', indent)
            else
                lines[#lines + 1] = string.format('%s%s = %s,', indent, kStr, vStr)
            end
        else
            print('key type error : '..kType, k)
        end
    end

    if depth == 1 then
        lines[#lines + 1] = ''
    end

    for i=1, num do
        local value = tbl[i]
        local vType = type(value)
        local vStr = (vType == 'string') and rvalueString(value) or tostring(value)

        if vType == 'table' then
            if depth == 1 then
                lines[#lines + 1] = string.format('%s{ ---------------------------------------- %02d', indent, i)
            else
                lines[#lines + 1] = string.format('%s{', indent)
            end
            table2stringRecursive(lines, value, depth + 1)
            lines[#lines + 1] = string.format('%s},', indent)
        else
            lines[#lines + 1] = string.format('%s%s,', indent, vStr)
        end

    end
end


-- table2string: 테이블을 Lua 코드 형태 문자열로 변환
-- 사용예: print(kutil.table2string({a=1, b={2,3}}))
function kutil.table2string(tbl, depth)
    depth = depth or 0
    local indent = string.rep('\t', depth)
    local lines = {indent..'{'}
    table2stringRecursive(lines, tbl, depth + 1)
    lines[#lines + 1] = indent..'}'
    return table.concat(lines, '\n')
end

return kutil
