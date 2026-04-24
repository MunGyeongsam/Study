-- ============================================================================
-- camera.lua — Unity 스타일 정사영 카메라
-- ============================================================================
--
-- ◆ 역할
--   orthographicSize 기반 2D 카메라. 월드 좌표 Y↑, 스크린 좌표 Y↓ 변환.
--   draw(fn) 안에서 모든 월드 렌더링이 이루어진다.
--
-- ◆ 핵심 API
--   camera.new(x, y, orthoSize) → cam
--   cam:draw(fn) — attach → fn() → detach (transform 적용)
--   cam:worldCoords(sx, sy) → wx, wy  (스크린→월드)
--   cam:cameraCoords(wx, wy) → sx, sy (월드→스크린)
--   cam:lookAt(x, y) / move / zoom / setOrthographicSize
--   cam:getPixelsPerUnit() — 스크린 px 당 월드 유닛
--
-- ◆ 좌표계
--   월드: 중심(0,0), Y↑, 단위=world units
--   스크린: 좌상단(0,0), Y↓, 단위=pixels

local cos, sin = math.cos, math.sin

local camera = {}
camera.__index = camera

local function new(x, y, orthographicSize, rot, viewportX, viewportY)
	x, y = x or 0, y or 0  -- 월드 중심 (0,0) 기본값
	orthographicSize = orthographicSize or 10  -- Unity 스타일: 카메라가 보는 월드의 절반 높이
	rot = rot or 0
	-- viewport: 카메라가 화면의 어느 위치를 중심으로 할지 (nil이면 화면 중심)
	return setmetatable({
		x = x, 
		y = y, 
		orthographicSize = orthographicSize, 
		rot = rot,
		viewportX = viewportX,  -- nil이면 자동으로 w/2 사용
		viewportY = viewportY   -- nil이면 자동으로 h/2 사용
	}, camera)
end

function camera:lookAt(x,y)
	self.x, self.y = x,y
	return self
end

function camera:move(x,y)
	self.x, self.y = self.x + x, self.y + y
	return self
end

function camera:pos()
	return self.x, self.y
end

function camera:rotate(phi)
	self.rot = self.rot + phi
	return self
end

function camera:rotateTo(phi)
	self.rot = phi
	return self
end

function camera:zoom(mul)
	self.orthographicSize = self.orthographicSize / mul  -- 크기가 작아지면 줌인
	return self
end

function camera:zoomTo(zoom)
	-- zoom = 1이면 기본 크기, zoom = 2면 2배 줌인
	self.orthographicSize = 10 / zoom  -- 기본 orthographicSize = 10 기준
	return self
end

-- Viewport 위치 설정 (카메라가 화면의 어느 위치를 중심으로 할지)
function camera:setViewport(x, y)
	self.viewportX = x
	self.viewportY = y
	return self
end

function camera:getViewport()
	return self.viewportX, self.viewportY
end

-- 화면 중심으로 viewport 설정
function camera:setViewportToCenter()
	self.viewportX = nil  -- nil로 설정하면 자동으로 중심 사용
	self.viewportY = nil
	return self
end

-- Unity 스타일 함수들
function camera:setOrthographicSize(size)
	self.orthographicSize = size
	return self
end

function camera:getOrthographicSize()
	return self.orthographicSize
end

-- 현재 픽셀당 월드 유닛 계산
function camera:getPixelsPerUnit()
	local _, screenHeight = love.graphics.getDimensions()
	return screenHeight / (self.orthographicSize * 2)
end

-- 월드 유닛당 픽셀 계산 
function camera:getUnitsPerPixel()
	return (self.orthographicSize * 2) / love.graphics.getHeight()
end

function camera:attach()
	local w, h = love.graphics.getWidth(), love.graphics.getHeight()
	love.graphics.push()
	
	-- Orthographic Size를 실제 스케일로 변환
	local scale = h / (self.orthographicSize * 2)
	
	-- 카메라 viewport 위치 (기본값: 화면 중심)
	local centerX = self.viewportX or w/2
	local centerY = self.viewportY or h/2
	
	-- 지정된 viewport 위치로 이동
	love.graphics.translate(centerX, centerY)
	
	-- Y축 반전과 줌을 한번에 적용 (수학적 좌표계)
	love.graphics.scale(scale, -scale)
	
	-- 카메라 회전 적용
	love.graphics.rotate(self.rot)
	
	-- 카메라 위치 적용 (월드 좌표)
	love.graphics.translate(-self.x, -self.y)
end

function camera:detach()
	love.graphics.pop()
end

function camera:draw(func)
	self:attach()
	func()
	self:detach()
end

function camera:cameraCoords(x,y)
	-- Orthographic Size 기반 좌표 변환
	local w, h = love.graphics.getWidth(), love.graphics.getHeight()
	local scale = h / (self.orthographicSize * 2)
	local c, s = cos(self.rot), sin(self.rot)
	
	-- Viewport 위치 (기본값: 화면 중심)
	local centerX = self.viewportX or w/2
	local centerY = self.viewportY or h/2
	
	x, y = x - self.x, y - self.y
	x, y = c * x - s * y, s * x + c * y
	return x * scale + centerX, -y * scale + centerY  -- Y축 반전
end

function camera:worldCoords(x, y)
	-- Orthographic Size 기반 좌표 변환
	local w, h = love.graphics.getWidth(), love.graphics.getHeight()
	local scale = h / (self.orthographicSize * 2)
	local c, s = cos(-self.rot), sin(-self.rot)
	
	-- Viewport 위치 (기본값: 화면 중심)
	local centerX = self.viewportX or w/2
	local centerY = self.viewportY or h/2
	
	x, y = (x - centerX) / scale, -(y - centerY) / scale  -- Y축 반전
	x, y = c * x - s * y, s * x + c * y
	return x + self.x, y + self.y
end

function camera:mousepos()
	return self:worldCoords(love.mouse.getPosition())
end

-- the module
return setmetatable({new = new},
	{__call = function(_, ...) return new(...) end})
