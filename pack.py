import zipfile
import ntpath
import glob
import sys
import os

workDir = os.getcwd() + "\\"

SEP = '\\'

ALL = 'all'
SERVER = 'server'
WIN = 'win'
PACK = 'pack'

DEVENV = '\"C:/Program Files (x86)/Microsoft Visual Studio 14.0/Common7/IDE/devenv.exe\"'


def zipdir(path, name=''):
	print ('Zipping ' + path + ' to ' + name)
	name = ntpath.basename(path) if name == '' else name
	zf = zipfile.ZipFile(name + '.zip', "w")

	for dirname, subdirs, files in os.walk(path):
		zf.write(dirname)
		for filename in files:
			zf.write(os.path.join(dirname, filename))

	zf.close()

def doCmd(s):
	print('Running: \'' + s + '\'...')
	os.popen(s).read()

def doAll():
	clear()

	buildWin()
	buildServer()

	pack()

def clear():
	files = glob.glob(workDir + '*.zip')
	for f in files:
		print ('Clearing ' + f)
		os.remove(f)


def buildProj(proj):
	SolutionName = workDir + 'Lines.sln'
	SolnConfigName = 'Release'
	doCmd(DEVENV + ' ' + SolutionName + ' /build ' + SolnConfigName + ' /project ' + proj)

def buildWin():
	buildProj('Lines')

def buildServer():
	buildProj('Lines.Server')

def pack():
	for task in tasks:
		if task == ALL:
			packAll()
		elif task == WIN:
			packWin()
		elif task == SERVER:
			packServer()

def packAll():
	packWin()
	packServer()

def packWin():
	zipdir('Lines' + SEP + 'bin' + SEP + 'Windows' + SEP + 'Release', 'win-' + version)

def packServer():
	zipdir('Lines.Server' + SEP + 'bin' + SEP + 'Release', 'server-' + version)

version = 'v0.7-alpha'
tasks = [ ALL ] if len(sys.argv) == 1 else sys.argv[1:]
needPack = False

for task in tasks:
	if task == ALL:
		doAll()
	elif task == CLEAR:
		clear()
	elif task == WIN:
		buildWin()
	elif task == SERVER:
		buildServer()	
	elif task == PACK:
		needPack = True
	else:
		print('Unknown task: ' + task)

if needPack:
	pack()