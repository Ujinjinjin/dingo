import argparse
import json

parser = argparse.ArgumentParser(description='Store variables between build and release steps', prog='store_variables')

parser.add_argument('--path', dest='path', help='Path of stored variables file', type=str)

if __name__ == '__main__':
    args = parser.parse_args()

    with open(args.path, 'r', encoding='utf8') as storage:
        variables = json.load(storage)

    print(f'##vso[task.setvariable variable=version]{variables["version"]}')
