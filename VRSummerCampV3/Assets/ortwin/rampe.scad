$fn=120;

r=25; //Radius Rampe
wandhoch=50; //Höhe der Rückwand, wird automatisch abgeschnitten
h=15; //Breite Rampe
ymax=18; //Größe in y-Richtung
wand=0.5; //Wandstärke überall
scale([10,10,10]) difference() {
    
    union() {
        //Rampe Unterfläche
        translate([h/2,0,r+.2]) rotate([0,90,0]) cylinder(r=r+wand,h=h,center=true);
        
        //Rückwand
        translate([h/2,ymax-wand/2,wandhoch/2]) cube([h,wand,wandhoch],center=true);
        
        //Boden rechts
        translate([h-3/2,ymax/2+4/2,wand/2]) cube([3,ymax-4,wand],center=true);
        
        //Boden links
        translate([3/2,ymax/2+4/2,wand/2]) cube([3,ymax-4,wand],center=true);
        
        //Seiteneinraster
        translate([h+0.7,10,wand/2]) cylinder(r=1,h=wand,center=true);
    }
    //Rampe Oberfläche
    translate([h/2,0,r+.2]) rotate([0,90,0]) cylinder(r=r,h=h+1,center=true);
    //ymax abschneiden
    translate([0,200+ymax,0]) cube([400,400,400],center=true);
    //ymin abschneiden
    translate([0,-200,0]) cube([400,400,400],center=true);
    //zmax abschneiden
    translate([0,0,200+20]) cube([400,400,400],center=true);
    //zmin abschneiden
    translate([0,0,-200]) cube([400,400,400],center=true);
    //Kante vorne abschneiden
    translate([0,0,+1.15]) rotate([30,0,0]) cube([400,2,2],center=true);
    //Seiteneinrasterloch
    translate([0.7,10,wand/2]) cylinder(r=1.05,h=wand+0.2,center=true);
}

