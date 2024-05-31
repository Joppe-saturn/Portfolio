let texture = [];
let cameraPosition = [0, 0, 0];
let objectPosition = [0, 0, 0];
let distanceToScreen = 250;
let cameraDirection = [0,0];
let roatedCamera = [0, 0, 0];
let objectDistance = [];
let objects = [];
let transProperties = [0,0,0]
let transXYSize = [];
let objectVisibility = [];
let screenPosition = [];
let fishPosition = [-300,-100,-200]
let totalFish = 100;
const screenSize = [1300, 800];
const bowlSize = [50,25,50];

function preload() {
  texture[0] = loadImage("assets/fish scale.png");
  texture[1] = loadImage("assets/steel.jpg");
  texture[2] = loadImage("assets/glass.png");
  texture[3] = loadImage("assets/sand.jpg");
  texture[4] = loadImage("assets/fish.png");
  texture[5] = loadImage("assets/clown-fish.png");
  texture[6] = loadImage("assets/gold-fish.png");
  texture[7] = loadImage("assets/shark.png");
  loadObjects()
}

function setup() {
  createCanvas(screenSize[0], screenSize[1]);
  for(let counter = 0; counter < totalFish; counter++) {
    fishPosition[counter] = [random(-2500,2500),random(-1000,1000),random(-2500,2500),random(0.1,2),random(-2,2),random(-2,2),random(-2,2),int(random(4,4))];
  }
}

function draw() {
  for(let counter = 0; counter < totalFish; counter++) {
    objects[counter] = [fishPosition[counter][0], fishPosition[counter][1], fishPosition[counter][2], fishPosition[counter][7], fishPosition[counter][3]];
  }
  background(220);
  cameraMovement(20);
  for(let counter = 0; counter < objects.length; counter++) {
    addObject(objects[counter][0], objects[counter][1], objects[counter][2], counter);
  }
  objectDistance.sort()
  for(let counter = 0; counter < objects.length; counter++) {
    //if(objectVisibility[objectDistance[counter][1]]) {
      screenPosition = [transXYSize[objectDistance[counter][1]][0],transXYSize[objectDistance[counter][1]][1],transXYSize[objectDistance[counter][1]][2],objects[objectDistance[counter][1]][3]];
      image(texture[screenPosition[3]],screenPosition[0],screenPosition[1],screenPosition[2],screenPosition[2]);
    //}
  }
  for(let counter = 0; counter < totalFish; counter++) {
    if(fishPosition[counter][0] < bowlSize[0]/2*100-100 && fishPosition[counter][0] > -(bowlSize[0]/2*100-100)) {
      fishPosition[counter][0] += 0.8 * fishPosition[counter][4];
    } else {
      fishPosition[counter][4] *= -1
      fishPosition[counter][0] += 0.8 * fishPosition[counter][4];
    }
    if(fishPosition[counter][1] < bowlSize[1]/2*100-100 && fishPosition[counter][1] > -(bowlSize[1]/2*100-100)) {
      fishPosition[counter][1] += 0.2 * fishPosition[counter][5];
    } else {
      fishPosition[counter][5] *= -1
      fishPosition[counter][1] += 0.2 * fishPosition[counter][5];
    }
    if(fishPosition[counter][2] < bowlSize[2]/2*100-100 && fishPosition[counter][2] > -(bowlSize[2]/2*100-100)) {
      fishPosition[counter][2] += 0.5 * fishPosition[counter][6];
    } else {
      fishPosition[counter][6] *= -1
      fishPosition[counter][2] += 0.5 * fishPosition[counter][6];
    }
  }
}

function projection3D(x, y, z, index) {
   if(z > 30 && z < 20000) {
     objectVisibility[index] = 1;
   } else {
     objectVisibility[index] = 0;
   }
  transProperties[0] = x * (distanceToScreen / z) + screenSize[0] / 2;
  transProperties[1] = y * (distanceToScreen / z) + screenSize[1] / 2;
  transProperties[2] = 100 * (distanceToScreen / z) * objects[index][4];
  transXYSize[index] = [transProperties[0], transProperties[1], transProperties[2]];
}


function cameraMovement(speed) {
  if(keyIsDown(83)) {
    cameraPosition[0] -= speed * sin(cameraDirection[1]);
    cameraPosition[2] -= speed * cos(cameraDirection[1]);
  }
  if(keyIsDown(87)) {
    cameraPosition[0] += speed * sin(cameraDirection[1]);
    cameraPosition[2] += speed * cos(cameraDirection[1]);
  }
  if(keyIsDown(68)) {
    cameraPosition[0] += speed * cos(cameraDirection[1]) * 2;
    cameraPosition[2] -= speed * sin(cameraDirection[1]) * 2;
  }
  if(keyIsDown(65)) {
    cameraPosition[0] -= speed * cos(cameraDirection[1]) * 2;
    cameraPosition[2] += speed * sin(cameraDirection[1]) * 2;
  }
  if(keyIsDown(16)) {
    cameraPosition[1] += speed * 2;
  }
  if(keyIsDown(32)) {
    cameraPosition[1] -= speed * 2;
  }
  if(keyIsDown(37)) {
    cameraDirection[1] -= 0.025;
  }
  if(keyIsDown(39)) {
    cameraDirection[1] += 0.025;
  }
  if(keyIsDown(38) && cameraDirection[0] > -1) {
    cameraDirection[0] -= 0.025;
  }
  if(keyIsDown(40) && cameraDirection[0] < 1) {
    cameraDirection[0] += 0.025;
  }
}

function addObject(x, y, z, index) {
  objectPosition = [x, y, z]
  rotationMatrixX((objectPosition[0] - cameraPosition[0]), (objectPosition[2] - cameraPosition[2]), 0 - cameraDirection[1]);
  rotationMatrixY((objectPosition[1] - cameraPosition[1]), roatedCamera[2], 0 - cameraDirection[0]);
  projection3D(roatedCamera[0], roatedCamera[1] , roatedCamera[2], index);
  sortObject(pow((objectPosition[0] - cameraPosition[0]),2) + pow((objectPosition[1] - cameraPosition[1]),2) + pow((objectPosition[2] - cameraPosition[2]),2), index);
}

function rotationMatrixX(x, z, direction) {
  roatedCamera[0] = z * sin(direction) + x * cos(direction);
  roatedCamera[2] = z * cos(direction) - x * sin(direction);
}

function rotationMatrixY(y, z, direction) {
  roatedCamera[1] = z * sin(direction) + y * cos(direction);
  roatedCamera[2] = z * cos(direction) - y * sin(direction);
}

function sortObject(distance, index) {
  objectDistance[index * objectVisibility[index]] = [1/(sqrt(distance)),index];
}

function loadObjects() {
  let objectTotal = totalFish
  for(let counter = 0; counter < bowlSize[0]; counter++) {
    objects[objectTotal] = [-(bowlSize[0] / 2 * 100) + 100 * counter, -(bowlSize[1] / 2 * 100), -(bowlSize[2] / 2 * 100),1,2];
    objectTotal++;
  }
  for(let counter = 0; counter < bowlSize[0]; counter++) {
    objects[objectTotal] = [-(bowlSize[0] / 2 * 100) + 100 * counter, -(bowlSize[1] / 2 * 100), (bowlSize[2] / 2 * 100),1,2];
    objectTotal++;
  }
  for(let counter = 0; counter < bowlSize[2]; counter++) {
    objects[objectTotal] = [-(bowlSize[0] / 2 * 100), -(bowlSize[1] / 2 * 100), -(bowlSize[2] / 2 * 100) + 100 * counter,1,2];
    objectTotal++;
  }
  for(let counter = 0; counter < bowlSize[2]; counter++) {
    objects[objectTotal] = [(bowlSize[0] / 2 * 100), -(bowlSize[1] / 2 * 100), -(bowlSize[2] / 2 * 100) + 100 * counter,1,2];
    objectTotal++;
  }
  for(let counter = 0; counter < bowlSize[0]; counter++) {
    objects[objectTotal] = [-(bowlSize[0] / 2 * 100) + 100 * counter, (bowlSize[1] / 2 * 100), -(bowlSize[2] / 2 * 100),1,2];
    objectTotal++;
  }
  for(let counter = 0; counter < bowlSize[0]; counter++) {
    objects[objectTotal] = [-(bowlSize[0] / 2 * 100) + 100 * counter, (bowlSize[1] / 2 * 100), (bowlSize[2] / 2 * 100),1,2];
    objectTotal++;
  }
  for(let counter = 0; counter < bowlSize[2]; counter++) {
    objects[objectTotal] = [-(bowlSize[0] / 2 * 100), (bowlSize[1] / 2 * 100), -(bowlSize[2] / 2 * 100) + 100 * counter,1,2];
    objectTotal++;
  }
  for(let counter = 0; counter < bowlSize[2]; counter++) {
    objects[objectTotal] = [(bowlSize[0] / 2 * 100), (bowlSize[1] / 2 * 100), -(bowlSize[2] / 2 * 100) + 100 * counter,1,2];
    objectTotal++;
  }
  for(let counter = 0; counter < bowlSize[1]; counter++) {
    objects[objectTotal] = [-(bowlSize[0] / 2 * 100), -(bowlSize[1] / 2 * 100) + 100 * counter, -(bowlSize[2] / 2 * 100),1,2];
    objectTotal++;
  }
  for(let counter = 0; counter < bowlSize[1]; counter++) {
    objects[objectTotal] = [-(bowlSize[0] / 2 * 100), -(bowlSize[1] / 2 * 100) + 100 * counter, (bowlSize[2] / 2 * 100),1,2];
    objectTotal++;
  }
  for(let counter = 0; counter < bowlSize[1]; counter++) {
    objects[objectTotal] = [(bowlSize[0] / 2 * 100), -(bowlSize[1] / 2 * 100) + 100 * counter, -(bowlSize[2] / 2 * 100),1,2];
    objectTotal++;
  }
  for(let counter = 0; counter < bowlSize[1]; counter++) {
    objects[objectTotal] = [(bowlSize[0] / 2 * 100), -(bowlSize[1] / 2 * 100) + 100 * counter, (bowlSize[2] / 2 * 100),1,2];
    objectTotal++;
  }
  for(let counter = 0; counter < (bowlSize[0] - 1); counter++) {
    for(let counter2 = 0; counter2 < (bowlSize[2] - 1); counter2++) {
      objects[objectTotal] = [-(bowlSize[0] / 2 * 100 - 100) + 100 * counter, (bowlSize[1] / 2 * 100), -(bowlSize[2] / 2 * 100 - 100) + 100 * counter2,3,3];
      objectTotal++;
    }
  }
  for(let counter = 0; counter < (bowlSize[1] - 1); counter++) {
    for(let counter2 = 0; counter2 < (bowlSize[2] - 1); counter2++) {
      objects[objectTotal] = [-(bowlSize[0] / 2 * 100), -(bowlSize[1] / 2 * 100 - 100) + 100 * counter, -(bowlSize[2] / 2 * 100 - 100) + 100 * counter2,2,1];
      objectTotal++;
    }
  }
  for(let counter = 0; counter < (bowlSize[1] - 1); counter++) {
    for(let counter2 = 0; counter2 < (bowlSize[2] - 1); counter2++) {
      objects[objectTotal] = [(bowlSize[0] / 2 * 100), -(bowlSize[1] / 2 * 100 - 100) + 100 * counter, -(bowlSize[2] / 2 * 100 - 100) + 100 * counter2,2,1];
      objectTotal++;
    }
  }
  for(let counter = 0; counter < (bowlSize[1] - 1); counter++) {
    for(let counter2 = 0; counter2 < (bowlSize[0] - 1); counter2++) {
      objects[objectTotal] = [-(bowlSize[0] / 2 * 100 - 100) + 100 * counter2, -(bowlSize[1] / 2 * 100 - 100) + 100 * counter, -(bowlSize[2] / 2 * 100),2,1];
      objectTotal++;
    }
  }
  for(let counter = 0; counter < (bowlSize[1] - 1); counter++) {
    for(let counter2 = 0; counter2 < (bowlSize[0] - 1); counter2++) {
      objects[objectTotal] = [-(bowlSize[0] / 2 * 100 - 100) + 100 * counter2, -(bowlSize[1] / 2 * 100 - 100) + 100 * counter, (bowlSize[2] / 2 * 100),2,1];
      objectTotal++;
    }
  }
}