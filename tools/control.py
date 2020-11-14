import argparse

parser = argparse.ArgumentParser(description='Create contents of control file', prog='control')

parser.add_argument('-p', '--pkg', dest='pkg', help='Package name', type=str, required=True)
parser.add_argument('-v', '--version', dest='version', help='Package version', type=str, required=True)
parser.add_argument('-s', '--size', dest='size', help='Installed size', type=str, required=True)
parser.add_argument('-d', '--description', dest='description', help='Target architecture', type=str, required=True)

if __name__ == '__main__':
    args = parser.parse_args()

    control_lines = [
        f'Package: {args.pkg}',
        f'Version: {args.version}',
        'Section: utils',
        'Priority: optional',
        'Architecture: all',
        'Essential: no',
        f'Installed-Size: {args.size}',
        'Maintainer: github.com/ujinjinjin',
        f'Description: {args.description}',
    ]

    print('\n'.join(control_lines))
