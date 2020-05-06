//////////////////////////////////////////////////////////////////////
// squareAnnulus1.cpp
//
// This program draws a square annulus as a triangle strip with
// vertices specified using glVertex3f() and colors using glColor3f().
//
// Interaction:
// Press the space bar to toggle between wireframe and filled.
//
// Sumanta Guha
//////////////////////////////////////////////////////////////////////

#include <iostream>
#include <vector>
#include <GL/glew.h>
#include <GL/freeglut.h> 
#include <glm.hpp>
struct Point {
public:
	glm::vec3 position;
	void Draw();
};


class Segment {
public:
	Point startPoint;
	Point endPoint;
	glm::vec3 edgeVector;
	Segment(Point start, Point End) {
		startPoint = start; endPoint = End;
		glm::vec3 startpos;
		glm::vec3 endpos;
		startpos.x = start.position.x;
		startpos.y = start.position.y;
		startpos.z = start.position.z;
		endpos.x = End.position.x;
		endpos.y = End.position.y;
		endpos.z = End.position.z;
		edgeVector = endpos - startpos;
	}
	
	void Draw();
private:
	
};
void Point::Draw()
{
	glPointSize(2.0f);
	glBegin(GL_POINTS);
	glVertex3f(position.x, position.y,position.z);
	glEnd();
}

void Segment::Draw()
{
	glBegin(GL_LINE);
	glVertex3f(startPoint.position.x, startPoint.position.y, startPoint.position.z);
	glVertex3f(endPoint.position.x, endPoint.position.y, endPoint.position.z);
	glEnd();
}

class Poligon
{
public:
	
	
	std::vector<Point> points;
	std::vector<Segment> segments;
	void Draw();

private:

};
void Poligon::Draw()
{
	glBegin(GL_LINE_LOOP);
	for (int i = 0; i < points.size(); i++)
	{
		glVertex3f(points.at(i).position.x, points.at(i).position.y, points.at(i).position.z);
	}
	glEnd();
}



// Globals.
float width, height;
static int isWire = 0; // Is wireframe?
std::vector<float> zValues;
std::vector<glm::vec3> rays;
Poligon poligon;
Point q;
// Drawing routine.
void drawScene(void)
{
	width = glutGet(GLUT_WINDOW_WIDTH);
	height = glutGet(GLUT_WINDOW_HEIGHT);
	glClear(GL_COLOR_BUFFER_BIT);
	glColor3f(1, 0, 0);
	if (isWire) glPolygonMode(GL_FRONT_AND_BACK, GL_LINE); else glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
	poligon.Draw();

	q.Draw();
	glColor3f(0, 0, 1);
	
	for (size_t i = 0; i < poligon.points.size(); i++)
	{
		glBegin(GL_LINES);
		glVertex3f(q.position.x, q.position.y, q.position.z);
		glVertex3f(poligon.points.at(i).position.x, poligon.points.at(i).position.y, poligon.points.at(i).position.z);
		glEnd();
	}
	

	glFlush();
}

// Initialization routine.
void setup(void)
{
	glClearColor(1.0, 1.0, 1.0, 0.0);
	for (int i = 0; i <4; i++)
	{
		Point p;
		if (i%3==0)
		{
			p.position.x = i * 4;
			p.position.y= i * i * 2;
			p.position.z = 0;
		}
		
		else
		{
			p.position.x = i * 7;
			p.position.y = -i * i - 5;
			p.position.z = 0;
		}
		
		poligon.points.push_back(p);
		
	}
	for (size_t i = 1; i < poligon.points.size(); i++)
	{
		Segment kenar{ poligon.points.at(i - 1),poligon.points.at(i) };
		poligon.segments.push_back(kenar);
	}
	Segment kenar{ poligon.points.at(poligon.points.size() - 1),poligon.points.at(0) };
	poligon.segments.push_back(kenar);
	
}

// OpenGL window reshape routine.
void resize(int w, int h)
{
	
	glViewport(0, 0, w, h);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	glOrtho(-250, 250, -250, 250, -1.0, 1.0);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
}

// Keyboard input processing routine.
void keyInput(unsigned char key, int x, int y)
{
	switch (key)
	{
	case ' ':
		if (isWire == 0) isWire = 1;
		else isWire = 0;
		glutPostRedisplay();
		break;
	
	case 27:
		exit(0);
		break;
	default:
		break;
	}
}
// Mouse callback routine.
void mouseControl(int button, int state, int x, int y)
{
	// Store the clicked point in the currentPoint variable when left button is pressed.
	if (button == GLUT_LEFT_BUTTON && state == GLUT_DOWN)
	{
		q.position.x = x-250;
		q.position.y = height-y-250;
		q.position.z = 0;
		
		//std::cout << q.X << " " << q.Y << std::endl;
		glutPostRedisplay();
	}
	if (button == GLUT_LEFT_BUTTON && state == GLUT_UP)
	{
		std::cout << "q noktasinin koordinatlari:" << std::endl;
		std::cout << "X: " << q.position.x << " Y: " << q.position.y << std::endl;
		rays.clear();
		zValues.clear();
		for (size_t i = 0; i < poligon.points.size(); i++)
		{
			glm::vec3 rayfrompoints;
			rayfrompoints = q.position - poligon.points.at(i).position;

			rays.push_back(rayfrompoints);
		}
		
		for (size_t i = 0; i < poligon.points.size(); i++)
		{
			zValues.push_back( glm::cross(poligon.segments.at(i).edgeVector, rays.at(i)).z);
		}
		std::cout << "Vektorel carpimlarin z bilesenleri:" << std::endl;
		int carpim=0;
		bool isaretDegisti = false;
		for (size_t i = 0; i < zValues.size(); i++)
		{
			std::cout << zValues.at(i) << std::endl;
			if (i > 0&&!isaretDegisti)
			{
				carpim = zValues.at(i) * zValues.at(i - 1);
				if (carpim<0)
				{
					
					isaretDegisti = true;
				}
				
			}
			
		}
		if (isaretDegisti)
		{
			std::cout << "q noktasi disarida" << std::endl;
		}
		else
		{
			std::cout << "q noktasi iceride" << std::endl;
		}

		
	}
		
	

	

	
}

// Mouse motion callback routine.
void mouseMotion(int x, int y)
{
	// Update the location of the current point as the mouse moves with button pressed.
	q.position.x = x-250;
	q.position.y = height-y-250;
	

	glutPostRedisplay();
}
// Routine to output interaction instructions to the C++ window.
void printInteraction(void)
{
	std::cout << "Interaction:" << std::endl;
	std::cout << "Press the space bar to toggle between wireframe and filled." << std::endl;
}

// Main routine.
int main(int argc, char** argv)
{
	printInteraction();
	glutInit(&argc, argv);

	glutInitContextVersion(4, 3);
	glutInitContextProfile(GLUT_COMPATIBILITY_PROFILE);

	glutInitDisplayMode(GLUT_SINGLE | GLUT_RGBA);
	glutInitWindowSize(500,500);
	glutInitWindowPosition(100, 100);
	glutCreateWindow("Odev3.cpp");
	glutDisplayFunc(drawScene);
	glutReshapeFunc(resize);
	glutKeyboardFunc(keyInput);
	// Register the mouse callback function.
	glutMouseFunc(mouseControl);

	// Register the mouse motion callback function.
	glutMotionFunc(mouseMotion);
	glewExperimental = GL_TRUE;
	glewInit();

	setup();

	glutMainLoop();

}

