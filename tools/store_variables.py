import argparse
import json

parser = argparse.ArgumentParser(description='Store variables between build and release steps', prog='store_variables')

parser.add_argument('-v', '--version', dest='version', help='Package version', type=str)
parser.add_argument('--dest', dest='destination', help='Destination where variables will be stored', type=str)

if __name__ == '__main__':
    args = parser.parse_args()

    variables = {
        'version': args.version
    }

    with open(args.destination, 'w', encoding='utf8') as storage:
        json.dump(variables, storage)
