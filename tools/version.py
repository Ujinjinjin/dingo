import argparse

parser = argparse.ArgumentParser(description='Get version of package', prog='version')

parser.add_argument('--major', dest='major', help='Major version number', type=int, required=True)
parser.add_argument('--minor', dest='minor', help='Minor version number', type=int, required=True)
parser.add_argument('--patch', dest='patch', help='Patch number', type=int, required=True)
parser.add_argument('--arch', dest='arch', help='Target architecture', type=str)
parser.add_argument('--dev', '-d',  dest='dev', help='Specify if dev version', action='store_true')

if __name__ == '__main__':
    args = parser.parse_args()

    version_suffix = ''
    if args.dev:
        version_suffix = '-dev'

    arch_suffix = ''
    if args.arch:
        arch_suffix = f'_{args.arch}'

    # 1.0.1_amd64-dev
    print(f'{args.major}.{args.minor}.{args.patch}{arch_suffix}{version_suffix}', end='')
