import c4d
#Welcome to the world of Python

spline_types = [ c4d.SPLINETYPE_LINEAR,  c4d.SPLINETYPE_CUBIC,  c4d.SPLINETYPE_BSPLINE,  c4d.SPLINETYPE_BEZIER]

def main():
    # Read the current path set in UserData
    path = op[c4d.ID_USERDATA,1]

    #Read the current Spline Type set in User Data
    spline_type = op[c4d.ID_USERDATA,2]

    # Open file and read lines
    f = open(path, "r")
    lines = f.readlines()
    points = []

    # Parse lines and construct points
    for line in lines:
       parts = line.split(",")
       x = float(parts[0])
       y = float(parts[1])
       z = float(parts[2])

       points.append(c4d.Vector(x,y,z))

    # Construct Spline Object and set its points
    polyline = c4d.SplineObject(len(points), spline_types[spline_type])
    polyline.SetAllPoints(points)


    return polyline