import argparse

parser = argparse.ArgumentParser(description='Get version of package', prog='version')

parser.add_argument('--major', dest='major', help='Major version number', type=int, required=True)
parser.add_argument('--minor', dest='minor', help='Minor version number', type=int, required=True)
parser.add_argument('--patch', dest='patch', help='Patch number', type=int, required=True)
parser.add_argument('--branch', dest='branch', help='Checked out branch', type=str)
parser.add_argument('--dev', '-d',  dest='dev', help='Specify if dev version', action='store_true')

if __name__ == '__main__':
    args = parser.parse_args()

    version_suffix = ''
    if args.dev or args.branch == 'main':
        version_suffix = '-dev'

    # 1.0.1_amd64-dev
    pkg_version = f'{args.major}.{args.minor}.{args.patch}{version_suffix}'
    print(f'##vso[task.setvariable variable=pkg_version]{pkg_version}') # used in this Job
    print(f'##vso[task.setvariable variable=pkgVersion;isOutput=true]{pkg_version}') # passed into further stages
