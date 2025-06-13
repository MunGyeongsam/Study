/*

class Point
{
	int x{ 0 };
	int y{ 0 };
public:
	// #1. implicit object parameter - C++98 ~ C++20
	void set_x(int value)	// void set_x(Point* this, int value)	
	{						// {
		x = value;			// 		this->x = value;
	}						// }

	// #2. explicit object parameter - C++23
	void set_y(this Point& self, int value)
	{
		self.y = value;
	}
};

int main()
{
	Point pt;
	pt.set_x(3);
	pt.set_y(3); // set_y(pt, 3)
}

//*/